using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PayFlow.DOMAIN.Infrastructure.Repositories
{
    public class TransaccionesRepository : ITransaccionesRepository
    {
        private readonly PayflowContext _context;

        public TransaccionesRepository(PayflowContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transacciones>> GetAllTransacciones()
        {
            return await _context.Transacciones.ToListAsync();
        }

        public async Task<Transacciones?> GetTransaccionById(int id)
        {
            return await _context.Transacciones.Where(c => c.TransaccionId == id).FirstOrDefaultAsync();
        }

        // Legacy: solo para usos antiguos, no para transferencias atómicas
        public async Task<int> AddTransaccion(Transacciones transaccion)
        {
            await _context.Transacciones.AddAsync(transaccion);
            await _context.SaveChangesAsync();
            return transaccion.TransaccionId;
        }

        // Para transferencias atómicas (NO llama SaveChangesAsync)
        public async Task<int> AddTransaccionAsync(Transacciones transaccion)
        {
            await _context.Transacciones.AddAsync(transaccion);
            await _context.SaveChangesAsync();
            return transaccion.TransaccionId;
        }

        // Para transferencias atómicas (NO llama SaveChangesAsync)
        public async Task AddRangeTransaccionesAsync(IEnumerable<Transacciones> transacciones)
        {
            await _context.Transacciones.AddRangeAsync(transacciones);
        }

        public async Task<bool> UpdateTransaccion(Transacciones transaccion)
        {
            var existingTransaccion = await GetTransaccionById(transaccion.TransaccionId);
            if (existingTransaccion == null)
            {
                return false;
            }
            existingTransaccion.CuentaId = transaccion.CuentaId;
            existingTransaccion.TipoTransaccion = transaccion.TipoTransaccion;
            existingTransaccion.Monto = transaccion.Monto;
            existingTransaccion.FechaHora = transaccion.FechaHora;
            existingTransaccion.Estado = transaccion.Estado;
            existingTransaccion.NumeroOperacion = transaccion.NumeroOperacion;
            existingTransaccion.Banco = transaccion.Banco;
            existingTransaccion.RutaVoucher = transaccion.RutaVoucher;
            existingTransaccion.ComentariosAdmin = transaccion.ComentariosAdmin;
            existingTransaccion.Comentario = transaccion.Comentario;
            existingTransaccion.CuentaDestinoId = transaccion.CuentaDestinoId;
            existingTransaccion.Iporigen = transaccion.Iporigen;
            existingTransaccion.Ubicacion = transaccion.Ubicacion;
            _context.Transacciones.Update(existingTransaccion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RechazarTransaccion(int id)
        {
            var transaccion = await GetTransaccionById(id);
            if (transaccion == null)
            {
                return false;
            }
            transaccion.Estado = "Rechazado";
            _context.Transacciones.Update(transaccion);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Transacciones>> GetTransaccionesByCuentaId(int cuentaId)
        {
            return await _context.Transacciones.Where(c => c.CuentaId == cuentaId).ToListAsync();
        }

        public async Task<int?> GetUltimoNumeroOperacionAsync()
        {
            var lastTransaccion = await _context.Transacciones
                                             .OrderByDescending(t => t.TransaccionId)
                                             .FirstOrDefaultAsync();
            if (lastTransaccion == null)
            {
                return 1242;
            }
            var numeroOperacion = lastTransaccion.NumeroOperacion;
            var numero = int.Parse(numeroOperacion.Substring(2));
            return numero;
        }

        public async Task<IEnumerable<Transacciones>> GetTransaccionesByUsuario(int usuarioId, string? estado = null, System.DateTime? fechaInicio = null, System.DateTime? fechaFin = null)
        {
            var query = _context.Transacciones
                .Include(t => t.Cuenta)
                .Where(t => t.Cuenta.UsuarioId == usuarioId);
            if (!string.IsNullOrEmpty(estado))
                query = query.Where(t => t.Estado == estado);
            if (fechaInicio.HasValue)
                query = query.Where(t => t.FechaHora >= fechaInicio.Value);
            if (fechaFin.HasValue)
            {
                var fin = fechaFin.Value;
                if (fin.TimeOfDay == System.TimeSpan.Zero)
                {
                    fin = fin.Date.AddDays(1).AddTicks(-1);
                }
                query = query.Where(t => t.FechaHora <= fin);
            }
            return await query.ToListAsync();
        }

        public async Task<ResumenInicioDTO?> ObtenerResumenInicioAsync(int usuarioId)
        {
            var cuenta = await _context.Cuentas
                .Where(c => c.UsuarioId == usuarioId && c.EstadoCuenta == "Activo")
                .FirstOrDefaultAsync();
            if (cuenta == null)
                return null;
            var movimientos = await _context.Transacciones
                .Where(t => t.CuentaId == cuenta.CuentaId)
                .OrderByDescending(t => t.FechaHora)
                .Take(10)
                .Select(t => new MovimientoDTO
                {
                    TipoTransaccion = t.TipoTransaccion,
                    NumeroOperacion = t.NumeroOperacion,
                    FechaHora = t.FechaHora,
                    Monto = t.Monto,
                    Estado = t.Estado
                })
                .ToListAsync();
            return new ResumenInicioDTO
            {
                NumeroCuenta = cuenta.NumeroCuenta,
                Saldo = cuenta.Saldo,
                Movimientos = movimientos
            };
        }
    }
}