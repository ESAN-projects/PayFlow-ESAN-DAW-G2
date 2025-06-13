using Microsoft.AspNetCore.Http;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class RetiroService : IRetiroService
    {
        public readonly ITransaccionesRepository _transaccionesRepository;
        public readonly INotificacionService _notificacionService;
        // private readonly ICuentasRepository _cuentasRepository;
        public RetiroService(ITransaccionesRepository transaccionesRepository, INotificacionService notificacionService)
        {
            _transaccionesRepository = transaccionesRepository;
            _notificacionService = notificacionService;
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
                Iporigen = transaccion.Iporigen
            };
            return retiroDTO;
        }

        //Add Draw transacciones
        public async Task<int> AddRetiro(RetiroCreateDTO retiroCreateDTO, string Iporigen)
        {
            // Validate the transaccion object before adding
            if (retiroCreateDTO.CuentaId <= 0 || retiroCreateDTO.Monto <= 0)
            {
                return -1; // Indicate an error with invalid data
            }

            var estado = "Aceptado"; // Default state for the transaction

            if (retiroCreateDTO.Monto > 100000)
            {
                estado = "Pendiente"; // If the amount is greater than 100000, set state to "Pendiente"
            }

            /*
            var cuenta = await _cuentasRepository.GetCuentaById(retiroCreateDTO.CuentaId);
            if (cuenta == null)
                throw new InvalidOperationException("Cuenta no encontrada.");

            // Validar saldo suficiente
            if (cuenta.Saldo < retiroCreateDTO.Monto)
                throw new InvalidOperationException("Saldo insuficiente para realizar el retiro.");
            */

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
                    UsuarioId = 1, //cuenta.UsuarioId, // Assuming you have the user ID from the cuenta object
                    TransaccionId = transactionId,
                    TipoNotificacion = "Alerta",
                    Mensaje = "Retiro pendiente de aprobación por monto elevado.",
                    FechaHora = DateTime.Now,
                    Estado = "No Leido"
                };
                await _notificacionService.AddNotificacion(notificacion);
            }

            return transactionId;
        }
    }
}
