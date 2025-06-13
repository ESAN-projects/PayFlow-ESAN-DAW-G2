using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.DTOs;
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

        public async Task<List<NotificacionDTO>> ObtenerNotificacionesPorUsuario(int usuarioId)
        {
            var notificaciones = await _context.Notificaciones
                .Include(n => n.Transaccion)
                .Where(n => n.UsuarioId == usuarioId)
                .OrderByDescending(n => n.FechaHora)
                .ToListAsync();

            List<NotificacionDTO> resultado = new List<NotificacionDTO>();
            foreach (var n in notificaciones)
            {
                NotificacionDTO dto = new NotificacionDTO();
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
    }
}
