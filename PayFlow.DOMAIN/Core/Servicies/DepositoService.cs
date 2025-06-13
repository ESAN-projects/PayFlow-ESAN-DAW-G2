using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Text.RegularExpressions;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class DepositoService : IDepositoService
    {
        private readonly ITransaccionesRepository _transaccionesRepository;
        private readonly ICuentasRepository _cuentasRepository;
        private readonly IFileService _fileService;

        public DepositoService(ITransaccionesRepository transaccionesRepository, ICuentasRepository cuentasRepository, IFileService fileService)
        {
            _transaccionesRepository = transaccionesRepository;
            _cuentasRepository = cuentasRepository;
            _fileService = fileService;
        }

        public async Task<DepositoDTO> RegistrarDepositoAsync(RegistrarDepositosDTO registrarDepositoDTO)
        {
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
                CuentaId = registrarDepositoDTO.CuentaID,
                TipoTransaccion = "Deposito",
                Monto = registrarDepositoDTO.Monto,
                FechaHora = DateTime.UtcNow,
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

            // Registrar la transacción en la base de datos
            var transaccionId = await _transaccionesRepository.AddTransaccion(transaccion);

            return new DepositoDTO
            {
                Monto = registrarDepositoDTO.Monto,
                FechaTransaccion = transaccion.FechaHora
            };
        }
    }
}
