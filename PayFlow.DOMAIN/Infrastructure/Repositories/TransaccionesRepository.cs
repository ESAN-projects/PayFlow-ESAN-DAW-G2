using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;

namespace PayFlow.DOMAIN.Infrastructure.Repositories
{
    public class TransaccionesRepository : ITransaccionesRepository
    {
        private readonly PayflowContext _context;
        public TransaccionesRepository(PayflowContext context)
        {
            _context = context;
        }

        //Get all transacciones
        public async Task<IEnumerable<Transacciones>> GetAllTransacciones()
        {
            return await _context.Transacciones.ToListAsync();
        }
        //Get transacciones by id
        public async Task<Transacciones?> GetTransaccionById(int id)
        {
            return await _context.Transacciones.Where(c => c.TransaccionId == id).FirstOrDefaultAsync();
        }
        //Add transacciones
        public async Task<int> AddTransaccion(Transacciones transaccion)
        {
            await _context.Transacciones.AddAsync(transaccion);
            await _context.SaveChangesAsync();
            return transaccion.TransaccionId;
        }
        //Update transacciones
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
        //Rechazar (Delete) transacciones
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

        //Get transacciones by cuentaId
        public async Task<IEnumerable<Transacciones>> GetTransaccionesByCuentaId(int cuentaId)
        {
            return await _context.Transacciones.Where(c => c.CuentaId == cuentaId).ToListAsync();
        }

        // Obtener el último número de operación registrado
        public async Task<int?> GetUltimoNumeroOperacionAsync()
        {
            var lastTransaccion = await _context.Transacciones
                                             .OrderByDescending(t => t.TransaccionId)
                                             .FirstOrDefaultAsync();
            if (lastTransaccion == null)
            {
                return 1242; // Si no hay registros, se empieza desde el número OP1243
            }

            var numeroOperacion = lastTransaccion.NumeroOperacion;
            // Extraer el número de operación (después de "OP")
            var numero = int.Parse(numeroOperacion.Substring(2));

            return numero;
        }
    }
}
