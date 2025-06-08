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
        Task<List<NotificacionDTO>> ObtenerNotificacionesPorUsuario(int usuarioId);
        Task MarcarComoLeido(int notificacionId);
    }
}
