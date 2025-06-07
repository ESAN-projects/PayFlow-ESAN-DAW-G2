using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PayFlow.DOMAIN.Infrastructure.Repositories
{
    public class CuentasRepository : ICuentasRepository
    {
        private readonly PayflowContext _context;

        public CuentasRepository(PayflowContext context)
        {
            _context = context;
        }

        public async Task<Cuentas?> ObtenerCuentaConTransaccionesAsync(int usuarioId)
        {
            var cuenta = await _context.Cuentas
                .Include(c => c.TransaccionesCuenta)
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
            if (cuenta != null && cuenta.TransaccionesCuenta != null)
            {
                cuenta.TransaccionesCuenta = cuenta.TransaccionesCuenta
                    .OrderByDescending(t => t.FechaHora)
                    .Take(5)
                    .ToList();
            }
            return cuenta;
        }
    }
}
