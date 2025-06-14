using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<NotificacionxUsuarioDTO>> ObtenerNotificacionesPorUsuario(int usuarioId)
        {
            var notificaciones = await _context.Notificaciones
                .Include(n => n.Transaccion)
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaHora)
                .ToListAsync();

            List<NotificacionxUsuarioDTO> resultado = new List<NotificacionxUsuarioDTO>();
            foreach (var n in notificaciones)
            {
                NotificacionxUsuarioDTO dto = new NotificacionxUsuarioDTO();
                dto.NotificacionID = n.NotificacionId;
                dto.TransaccionID = n.TransaccionId.HasValue ? n.TransaccionId.Value : 0;
                dto.TipoTransaccion = n.TipoNotificacion;
                dto.Monto = n.Transaccion != null ? n.Transaccion.Monto : 0;
                dto.FechaHora = n.FechaHora;
                dto.Mensaje = n.Mensaje;
                dto.Estado = n.Estado;
                resultado.Add(dto);
            }
            return resultado;
        }

        public async Task MarcarComoLeido(int notificacionId)
        {
            var notificacion = await _context.Notificaciones.FindAsync(notificacionId);
            if (notificacion != null)
            {
                notificacion.Estado = "Leido";
                await _context.SaveChangesAsync();
            }
        }

        //Get all notificaiones
        public async Task<IEnumerable<Notificacion>> GetAllNotificaciones()
        {
            return await _context.Notificaciones.ToListAsync();
        }
        //Get notificacion by id
        public async Task<Notificacion> GetNotificacionById(int id)
        {
            return await _context.Notificaciones.Where(c => c.NotificacionId == id).FirstOrDefaultAsync();
        }
        //Add notificacion
        public async Task<int> AddNotificacion(Notificacion notificacion)
        {
            await _context.Notificaciones.AddAsync(notificacion);
            await _context.SaveChangesAsync();
            return notificacion.NotificacionId;
        }

        //Update notificacion
        public async Task<bool> UpdateNotificacion(Notificacion notificacion)
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
