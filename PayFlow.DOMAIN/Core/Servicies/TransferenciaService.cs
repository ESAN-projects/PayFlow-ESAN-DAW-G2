using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class TransferenciaService : ITransferenciaService
    {
        private readonly ICuentasRepository _cuentasRepository;
        private readonly ITransaccionesRepository _transaccionesRepository;
        private readonly PayflowContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransferenciaService(
            ICuentasRepository cuentasRepository,
            ITransaccionesRepository transaccionesRepository,
            PayflowContext context,
            IHttpContextAccessor httpContextAccessor)
        {
            _cuentasRepository = cuentasRepository;
            _transaccionesRepository = transaccionesRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TransferenciaResponseDto> RealizarTransferenciaAsync(TransferenciaRequestDto requestDto, string cuentaOrigenNumero)
        {
            if (cuentaOrigenNumero == requestDto.CuentaDestinoNumero)
            {
                return new TransferenciaResponseDto { Success = false, Message = "La cuenta de origen y destino no pueden ser la misma." };
            }

            var cuentaOrigen = await _cuentasRepository.ObtenerPorNumeroCuentaAsync(cuentaOrigenNumero);
            var cuentaDestino = await _cuentasRepository.ObtenerPorNumeroCuentaAsync(requestDto.CuentaDestinoNumero);

            if (cuentaOrigen == null)
                return new TransferenciaResponseDto { Success = false, Message = "La cuenta de origen no existe." };
            if (cuentaDestino == null)
                return new TransferenciaResponseDto { Success = false, Message = "La cuenta de destino no existe." };
            if (cuentaOrigen.EstadoCuenta != "Activo")
                return new TransferenciaResponseDto { Success = false, Message = "La cuenta de origen no está activa." };
            if (cuentaDestino.EstadoCuenta != "Activo")
                return new TransferenciaResponseDto { Success = false, Message = "La cuenta de destino no está activa." };
            if (requestDto.Monto <= 0)
                return new TransferenciaResponseDto { Success = false, Message = "El monto debe ser mayor a cero." };
            if (cuentaOrigen.Saldo < requestDto.Monto)
                return new TransferenciaResponseDto { Success = false, Message = "Saldo insuficiente en la cuenta de origen." };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Actualizar saldos
                cuentaOrigen.Saldo -= requestDto.Monto;
                cuentaDestino.Saldo += requestDto.Monto;
                await _cuentasRepository.UpdateCuentaAsync(cuentaOrigen);
                await _cuentasRepository.UpdateCuentaAsync(cuentaDestino);

                // Generar NumeroOperacion tipo OP{N}
                var ultimoNumeroOperacion = await _transaccionesRepository.GetUltimoNumeroOperacionAsync();
                var nuevoNumeroOperacion = $"OP{(ultimoNumeroOperacion.GetValueOrDefault() + 1)}";
                var ipOrigen = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();

                // Crear transacciones
                var transaccionDebito = new Transacciones
                {
                    CuentaId = cuentaOrigen.CuentaId,
                    TipoTransaccion = "Transferencia",
                    Monto = requestDto.Monto,
                    FechaHora = DateTime.UtcNow,
                    Estado = "Aceptado",
                    NumeroOperacion = nuevoNumeroOperacion,
                    Comentario = null,
                    CuentaDestinoId = cuentaDestino.CuentaId,
                    Iporigen = ipOrigen,
                    Ubicacion = null
                };
                var transaccionCredito = new Transacciones
                {
                    CuentaId = cuentaDestino.CuentaId,
                    TipoTransaccion = "Deposito",
                    Monto = requestDto.Monto,
                    FechaHora = DateTime.UtcNow,
                    Estado = "Aceptado",
                    NumeroOperacion = nuevoNumeroOperacion,
                    Comentario = $"Transferencia recibida de {cuentaOrigenNumero}",
                    CuentaDestinoId = null,
                    Iporigen = ipOrigen,
                    Ubicacion = null
                };

                await _transaccionesRepository.AddRangeTransaccionesAsync(new List<Transacciones> { transaccionDebito, transaccionCredito });
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new TransferenciaResponseDto
                {
                    Success = true,
                    Message = "Transferencia realizada con éxito.",
                    NumeroOperacion = nuevoNumeroOperacion,
                    MontoTransferido = requestDto.Monto
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new TransferenciaResponseDto
                {
                    Success = false,
                    Message = "Ocurrió un error inesperado al procesar la transferencia. Por favor, intente de nuevo más tarde."
                };
            }
        }
    }
}