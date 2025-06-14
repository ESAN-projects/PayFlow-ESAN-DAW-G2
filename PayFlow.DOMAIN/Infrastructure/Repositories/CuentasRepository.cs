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

        // Obtener una cuenta por su ID
        public async Task<Cuentas?> GetCuentaByIdAsync(int cuentaId)
        {
            return await _context.Cuentas.FirstOrDefaultAsync(c => c.CuentaId == cuentaId);
        }

        // Actualizar el saldo de una cuenta
        public async Task<bool> UpdateCuentaAsync(Cuentas cuenta)
        {
            var local = _context.Cuentas.Local.FirstOrDefault(e => e.CuentaId == cuenta.CuentaId);
            if (local != null && local != cuenta)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.Entry(cuenta).State = EntityState.Modified;
            return await _context.SaveChangesAsync() >= 0;
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
        public async Task<Cuentas?> GetCuentaByUsuarioId(int usuarioId)
        {
            var cuenta = await _context.Cuentas
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
            return cuenta;
        }
    }
}
