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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class RetiroService : IRetiroService
    {
        public readonly ITransaccionesRepository _transaccionesRepository;
        public readonly INotificacionService _notificacionService;
        private readonly ICuentasService _cuentasService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RetiroService(ITransaccionesRepository transaccionesRepository, INotificacionService notificacionService, ICuentasService cuentasService, IHttpContextAccessor httpContextAccessor)
        {
            _transaccionesRepository = transaccionesRepository;
            _notificacionService = notificacionService;
            _cuentasService = cuentasService;
            _httpContextAccessor = httpContextAccessor;
        }

        
        //Get draw transacciones by id
        public async Task<RetiroDTO?> GetRetiroById(int transactionId)
        {
            var transaccion = await _transaccionesRepository.GetTransaccionById(transactionId);
            if (transaccion == null)
            {
                return null;
            }
            if (transaccion.TipoTransaccion != "Retiro")
            {
                throw new InvalidOperationException("La transacción no es un retiro.");
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
            var usuarioIdString = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var usuarioId = int.Parse(usuarioIdString);
            if (usuarioId == null)
            {
                throw new Exception("Usuario no autenticado.");
            }
            // Obtener la cuenta asociada al usuario autenticado
            var cuentaUser = await _cuentasService.GetCuentaByUsuarioId(usuarioId);
            if (cuentaUser == null)
            {
                throw new InvalidOperationException("Cuenta no encontrada para el usuario autenticado.");
            }

            // Validar monto
            if (retiroCreateDTO.Monto <= 1)
            {
                throw new ArgumentException("Monto debe ser mayor a 1 Sol.");
            }

            var estado = "Aceptado";
            if (retiroCreateDTO.Monto > 100000)
            {
                estado = "Pendiente";
            }

            var cuenta = await _cuentasService.GetCuentaById(cuentaUser.CuentaId);
            if (cuenta == null)
                throw new InvalidOperationException("Cuenta no encontrada.");

            // Validar saldo suficiente
            if (cuenta.Saldo < retiroCreateDTO.Monto)
                throw new InvalidOperationException("Saldo insuficiente para realizar el retiro.");

            // Obtener el último numeroOperacion
            var ultimoNumeroOperacion = await _transaccionesRepository.GetUltimoNumeroOperacionAsync();
            var nuevoNumeroOperacion = $"OP{ultimoNumeroOperacion + 1}";

            var transaccion = new Transacciones
            {
                CuentaId = cuentaUser.CuentaId,
                TipoTransaccion = "Retiro",
                Monto = retiroCreateDTO.Monto,
                FechaHora = DateTime.Now,
                Estado = estado,
                Iporigen = Iporigen,
                NumeroOperacion = nuevoNumeroOperacion
            };

            var transactionId = await _transaccionesRepository.AddTransaccionAsync(transaccion);

            if (retiroCreateDTO.Monto > 100000)
            {
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
