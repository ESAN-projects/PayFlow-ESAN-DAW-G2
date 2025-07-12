using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface INotificacionesRepository
    {
        Task<List<NotificacionxUsuarioDTO>> ObtenerNotificacionesPorUsuario(int usuarioId);
        Task MarcarComoLeido(int notificacionId);

        Task<int> AddNotificacion(Notificacion notificacion);
        Task<bool> DeleteNotificacion(int id);
        Task<IEnumerable<Notificacion>> GetAllNotificaciones();
        Task<Notificacion> GetNotificacionById(int id);
        Task<bool> UpdateNotificacion(Notificacion notificacion);
        Task<int> ContarNuevas(int usuarioId);
        Task<bool> MarcarComoLeidaAsync(int notificacionId, int usuarioId);
        Task<bool> MarcarTodasComoLeidasAsync(int usuarioId);

    }
}
