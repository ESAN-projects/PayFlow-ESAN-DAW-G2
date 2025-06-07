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

        public UsuarioDashboardService(ICuentasRepository cuentasRepository)
        {
            _cuentasRepository = cuentasRepository;
        }

        public async Task<DashboardDTO> ObtenerDashboardAsync(int usuarioId)
        {
            var cuenta = await _cuentasRepository.ObtenerCuentaConTransaccionesAsync(usuarioId);
            if (cuenta == null)
                throw new Exception("Cuenta no encontrada.");

            var transacciones = cuenta.TransaccionesCuenta
                .OrderByDescending(t => t.FechaHora)
                .Take(5)
                .Select(t => new TransaccionResumenDTO
                {
                    Fecha = t.FechaHora,
                    TipoTransaccion = t.TipoTransaccion,
                    Monto = t.Monto,
                    Estado = t.Estado
                }).ToList();

            return new DashboardDTO
            {
                SaldoActual = cuenta.Saldo,
                UltimasTransacciones = transacciones
            };
        }
    }
}
