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
    public class HistorialValidacionesRepository : IHistorialValidacionesRepository
    {
        private readonly PayflowContext _context;

        public HistorialValidacionesRepository(PayflowContext context)
        {
            _context = context;
        }

        // Registrar una validación manual
        public async Task RegistrarValidacionManualAsync(HistorialValidaciones validacion)
        {
            await _context.HistorialValidaciones.AddAsync(validacion);
            await _context.SaveChangesAsync();
        }

        // Obtener historial de validaciones con filtros
        public async Task<IEnumerable<HistorialValidaciones>> ObtenerHistorialAsync(FiltroValidacionDTO filtro)
        {
            var query = _context.HistorialValidaciones
                .Include(v => v.Transaccion)
                    .ThenInclude(t => t.Cuenta)
                .AsQueryable();

            if (filtro.FechaInicio.HasValue)
                query = query.Where(v => v.FechaHora >= filtro.FechaInicio.Value);

            if (filtro.FechaFin.HasValue)
                query = query.Where(v => v.FechaHora <= filtro.FechaFin.Value);

            if (!string.IsNullOrEmpty(filtro.Estado))
                query = query.Where(v => v.Resultado == filtro.Estado);

            if (filtro.ClienteId.HasValue)
                query = query.Where(v => v.Transaccion.Cuenta.UsuarioId == filtro.ClienteId);

            if (!string.IsNullOrEmpty(filtro.NumeroComprobante))
                query = query.Where(v => v.Transaccion.NumeroOperacion == filtro.NumeroComprobante);

            return await query.ToListAsync();
        }
    }
}

