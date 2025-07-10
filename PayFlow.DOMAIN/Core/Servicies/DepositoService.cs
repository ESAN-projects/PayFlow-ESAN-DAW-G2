using Microsoft.AspNetCore.Http;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class DepositoService : IDepositoService
    {
        private readonly ITransaccionesRepository _transaccionesRepository;
        private readonly ICuentasRepository _cuentasRepository;
        private readonly IFileService _fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepositoService(ITransaccionesRepository transaccionesRepository, ICuentasRepository cuentasRepository, IFileService fileService, IHttpContextAccessor httpContextAccessor)
        {
            _transaccionesRepository = transaccionesRepository;
            _cuentasRepository = cuentasRepository;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DepositoDTO> RegistrarDepositoAsync(RegistrarDepositosDTO registrarDepositoDTO)
        {
            try
            {
                // Obtener el UsuarioID desde el JWT
                var usuarioId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (usuarioId == null)
                {
                    throw new Exception("Usuario no autenticado.");
                }

                // Convertir UsuarioID a entero
                int userId = int.Parse(usuarioId);

                // Obtener el CuentaID asociado al UsuarioID
                var cuenta = await _cuentasRepository.GetCuentaByUsuarioIdAsync(userId);
                if (cuenta == null)
                {
                    throw new Exception("Cuenta no encontrada para el usuario.");
                }

                // Validar monto
                if (registrarDepositoDTO.Monto <= 1)
                {
                    throw new Exception("El monto debe ser mayor a S/1.");
                }

                // Validar que el monto tenga como máximo 2 decimales
                if (!Regex.IsMatch(registrarDepositoDTO.Monto.ToString(), @"^\d+(\.\d{1,2})?$"))
                {
                    throw new Exception("El monto solo puede tener hasta 2 decimales.");
                }

                // Obtener el último número de operación y aumentar en 1
                var ultimoNumeroOperacion = await _transaccionesRepository.GetUltimoNumeroOperacionAsync();
                var nuevoNumeroOperacion = $"OP{ultimoNumeroOperacion + 1}";

                // Validar archivo adjunto (voucher)
                var comprobanteUrl = await _fileService.SaveVoucherAsync(registrarDepositoDTO.RutaVoucher, nuevoNumeroOperacion);

                // Crear la transacción de depósito con estado "Pendiente"
                var transaccion = new Transacciones
                {
                    CuentaId = cuenta.CuentaId,
                    TipoTransaccion = "Deposito",
                    Monto = registrarDepositoDTO.Monto,
                    FechaHora = DateTime.Now,
                    Estado = "Pendiente",  // Estado inicial
                    NumeroOperacion = nuevoNumeroOperacion,
                    Banco = registrarDepositoDTO.Banco,
                    RutaVoucher = comprobanteUrl,
                    ComentariosAdmin = registrarDepositoDTO.ComentariosAdmin,
                    Comentario = registrarDepositoDTO.Comentario,
                    CuentaDestinoId = registrarDepositoDTO.CuentaDestinoID,
                    Iporigen = registrarDepositoDTO.IPOrigen,
                    Ubicacion = registrarDepositoDTO.Ubicacion
                };

                // Registrar la transacción en la base de datos a través del repositorio.
                // Es crucial que ITransaccionesRepository.AddTransaccionAsync (en su implementación)
                // llame a _context.SaveChangesAsync() y devuelva el TransaccionId generado por la DB.
                int transaccionIdGenerada = await _transaccionesRepository.AddTransaccionAsync(transaccion);

                // Devolver un DTO con la información del depósito registrado.
                // El 'transaccion.TransaccionId' debería estar poblado con el ID real después de la operación de AddTransaccionAsync.
                return new DepositoDTO
                {
                    Monto = registrarDepositoDTO.Monto,
                    NumeroOperacion = nuevoNumeroOperacion,
                    FechaTransaccion = transaccion.FechaHora,
                    TransaccionId = transaccion.TransaccionId // O transaccionIdGenerada, si prefieres usar el valor devuelto explícitamente
                };
            }
            catch (Exception ex)
            {
                // Manejo centralizado de excepciones:
                // 1. Loggear el error completo para depuración.
                Console.WriteLine($"Error al registrar el depósito: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");

                // 2. Relanzar una nueva excepción con un mensaje genérico
                //    o devolver un DTO de respuesta con el error,
                //    evitando exponer detalles internos de la implementación.
                throw new Exception("Ocurrió un error inesperado al procesar su solicitud de depósito. Por favor, intente de nuevo más tarde.", ex);
            }
        }
    }
}

