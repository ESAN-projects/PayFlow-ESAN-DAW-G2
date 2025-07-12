using PayFlow.DOMAIN.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface INotificacionService
    {
        Task<List<NotificacionxUsuarioDTO>> ObtenerNotificacionesPorUsuario(int usuarioId);
        Task MarcarComoLeido(int notificacionId);

        Task<int> AddNotificacion(NotificacionCreateDTO notificacionDTO);
        Task<bool> DeleteNotificacion(int id);
        Task<IEnumerable<NotificacionListDTO>> GetAllNotificaciones();
        Task<NotificacionListDTO> GetNotificacionById(int id);
        Task<bool> UpdateNotificacion(NotificacionDTO data);
        Task<int> ContarNotificacionesNoLeidas(int usuarioId);
        Task<bool> MarcarNotificacionComoLeidaAsync(int notificacionId, int usuarioId);
        Task<bool> MarcarTodasComoLeidasAsync(int usuarioId);

    }
}
