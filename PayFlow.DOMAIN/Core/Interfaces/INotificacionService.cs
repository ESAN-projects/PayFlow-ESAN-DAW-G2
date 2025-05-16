using PayFlow.DOMAIN.Core.DTOs;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface INotificacionService
    {
        Task<int> AddNotificacion(NotificacionCreateDTO notificacionDTO);
        Task<bool> DeleteNotificacion(int id);
        Task<IEnumerable<NotificacionListDTO>> GetAllNotificaciones();
        Task<NotificacionListDTO> GetNotificacionById(int id);
        Task<bool> UpdateNotificacion(NotificacionListDTO dato);
    }
}