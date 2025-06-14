using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class RetiroService : IRetiroService
    {
        public readonly ITransaccionesRepository _transaccionesRepository;
        public readonly INotificacionService _notificacionService;
        private readonly ICuentasService _cuentasService;
        public RetiroService(ITransaccionesRepository transaccionesRepository, INotificacionService notificacionService, ICuentasService cuentasService)
        {
            _transaccionesRepository = transaccionesRepository;
            _notificacionService = notificacionService;
            _cuentasService = cuentasService;
        }

        
        //Get draw transacciones by id
        public async Task<RetiroDTO> GetRetiroById(int transactionId)
        {
            var transaccion = await _transaccionesRepository.GetTransaccionById(transactionId);
            if (transaccion == null)
            {
                return null;
            }
            var retiroDTO = new RetiroDTO
            {
                TransaccionId = transaccion.TransaccionId,
                CuentaId = transaccion.CuentaId,
                Monto = transaccion.Monto,
                Iporigen = transaccion.Iporigen,
                Estado = transaccion.Estado
            };
            return retiroDTO;
        }

        //Add Draw transacciones
        public async Task<int> AddRetiro(RetiroCreateDTO retiroCreateDTO, string Iporigen)
        {
            // Validate the transaccion object before adding
            if (retiroCreateDTO.CuentaId <= 0 || retiroCreateDTO.Monto <= 0)
            {
                throw new ArgumentException("CuentaId y Monto deben ser validos.");
            }

            var estado = "Aceptado"; // Default state for the transaction

            if (retiroCreateDTO.Monto > 100000)
            {
                estado = "Pendiente"; // If the amount is greater than 100000, set state to "Pendiente"
            }

            
            var cuenta = await _cuentasService.GetCuentaById(retiroCreateDTO.CuentaId) ??
                throw new InvalidOperationException("Cuenta no encontrada.");

            // Validar saldo suficiente
            if (cuenta.Saldo < retiroCreateDTO.Monto)
                throw new InvalidOperationException("Saldo insuficiente para realizar el retiro.");

            var transaccion = new Transacciones
            {
                CuentaId = retiroCreateDTO.CuentaId,
                TipoTransaccion = "Retiro",
                Monto = retiroCreateDTO.Monto,
                FechaHora = DateTime.Now,
                Estado = estado,
                Iporigen = Iporigen
            };

            var transactionId = await _transaccionesRepository.AddTransaccion(transaccion);

            if (retiroCreateDTO.Monto > 100000)
            {
                // registramos mensaje de notificación
                var notificacion = new NotificacionCreateDTO
                {
                    UsuarioId = cuenta.UsuarioId,
                    TransaccionId = transactionId,
                    TipoNotificacion = "Alerta",
                    Mensaje = "Retiro pendiente de aprobación por monto elevado.",
                    FechaHora = DateTime.Now,
                    Estado = "No Leido"
                };
                await _notificacionService.AddNotificacion(notificacion);
            }

            // Actualizar el saldo de la cuenta con el monto del retiro
            cuenta.Saldo -= retiroCreateDTO.Monto;
            var resultCuenta = await _cuentasService.UpdateCuenta(cuenta);

            if (!resultCuenta)
            {
                throw new Exception("No se pudo actualizar el saldo de la cuenta.");
            }

            return transactionId;
        }
    }
}
