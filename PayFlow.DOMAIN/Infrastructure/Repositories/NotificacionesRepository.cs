using Microsoft.EntityFrameworkCore;
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
    public class NotificacionesRepository : INotificacionesRepository
    {

        private readonly PayflowContext _context;
        public NotificacionesRepository(PayflowContext context)
        {
            _context = context;
        }

        //Get all notificaiones
        public async Task<IEnumerable<Notificaciones>> GetAllNotificaciones()
        {
            return await _context.Notificaciones.ToListAsync();
        }
        //Get notificacion by id
        public async Task<Notificaciones> GetNotificacionById(int id)
        {
            return await _context.Notificaciones.Where(c => c.NotificacionId == id).FirstOrDefaultAsync();
        }
        //Add notificacion
        public async Task<int> AddNotificacion(Notificaciones notificacion)
        {
            await _context.Notificaciones.AddAsync(notificacion);
            await _context.SaveChangesAsync();
            return notificacion.NotificacionId;
        }

        //Update notificacion
        public async Task<bool> UpdateNotificacion(Notificaciones notificacion)
        {
            var existingNotificacion = await GetNotificacionById(notificacion.NotificacionId);
            if (existingNotificacion == null)
            {
                return false;
            }
            existingNotificacion.TipoNotificacion = notificacion.TipoNotificacion;
            existingNotificacion.Mensaje = notificacion.Mensaje;
            existingNotificacion.Estado = notificacion.Estado;
            existingNotificacion.Transaccion = notificacion.Transaccion;
            existingNotificacion.Usuario = notificacion.Usuario;

            await _context.SaveChangesAsync();
            return true;
        }

        //Delete notificacion
        public async Task<bool> DeleteNotificacion(int id)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);
            if (notificacion == null)
            {
                return false;
            }
            _context.Notificaciones.Remove(notificacion);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
