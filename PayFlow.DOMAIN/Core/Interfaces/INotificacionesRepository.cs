using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface INotificacionesRepository
    {
        Task<int> AddNotificacion(Notificaciones notificacion);
        Task<bool> DeleteNotificacion(int id);
        Task<IEnumerable<Notificaciones>> GetAllNotificaciones();
        Task<Notificaciones> GetNotificacionById(int id);
        Task<bool> UpdateNotificacion(Notificaciones notificacion);
    }
}