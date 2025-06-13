using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;

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
        public async Task<Cuentas> GetCuentaByIdAsync(int cuentaId)
        {
            return await _context.Cuentas.FirstOrDefaultAsync(c => c.CuentaId == cuentaId);
        }

        // Actualizar el saldo de una cuenta
        public async Task<bool> UpdateCuentaAsync(Cuentas cuenta)
        {
            _context.Cuentas.Update(cuenta);
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
