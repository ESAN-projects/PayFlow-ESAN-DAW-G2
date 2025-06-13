using PayFlow.DOMAIN.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface INotificacionesRepository
    {
        Task<List<NotificacionDTO>> ObtenerNotificacionesPorUsuario(int usuarioId);
        Task MarcarComoLeido(int notificacionId);
    }
}
