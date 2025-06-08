using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class UsuarioDashboardService : IUsuarioDashboardService
    {
        private readonly ICuentasRepository _cuentasRepository;
        private readonly IUsuariosRepository _usuariosRepository;

        public UsuarioDashboardService(ICuentasRepository cuentasRepository, IUsuariosRepository usuariosRepository)
        {
            _cuentasRepository = cuentasRepository;
            _usuariosRepository = usuariosRepository;
        }

        public async Task<DashboardDTO> ObtenerDashboardAsync(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new Exception("ID de usuario inválido.");

            var usuario = await _usuariosRepository.GetUsuarioByIdAsync(usuarioId);
            if (usuario == null)
                throw new Exception("Usuario no encontrado o inactivo.");

            var cuenta = await _cuentasRepository.ObtenerCuentaConTransaccionesAsync(usuarioId);
            if (cuenta == null)
                throw new Exception("Cuenta no encontrada.");

            if (!string.Equals(cuenta.EstadoCuenta, "Activo", StringComparison.OrdinalIgnoreCase))
                throw new Exception("Cuenta deshabilitada.");

            var transacciones = cuenta.TransaccionesCuenta.OrderByDescending(t => t.FechaHora).Take(5).Select(t => new TransaccionResumenDTO { Fecha = t.FechaHora, TipoTransaccion = t.TipoTransaccion, Monto = t.Monto, Estado = t.Estado }).ToList();

            return new DashboardDTO
            {
                SaldoActual = cuenta.Saldo,
                UltimasTransacciones = transacciones
            };
        }
    }
}
