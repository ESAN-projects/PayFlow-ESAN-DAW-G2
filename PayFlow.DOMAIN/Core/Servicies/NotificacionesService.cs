using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionesRepository _notificacionesRepository;

        public NotificacionService(INotificacionesRepository notificacionesRepository)
        {
            _notificacionesRepository = notificacionesRepository;
        }

        //Get all notifications for a user

        public async Task<List<NotificacionDTO>> ObtenerNotificacionesPorUsuario(int usuarioId)
        {
            var entidades = await _notificacionesRepository.ObtenerNotificacionesPorUsuario(usuarioId);

            var dtos = new List<NotificacionDTO>();

            foreach (var entidad in entidades)
            {
                var dto = new NotificacionDTO
                {
                    NotificacionID = entidad.NotificacionID,
                    TipoTransaccion = entidad.TipoTransaccion,
                    Monto = entidad.Monto,
                    FechaHora = entidad.FechaHora,
                    Mensaje = entidad.Mensaje,
                    Estado = entidad.Estado,
                };

                dtos.Add(dto);
            }

            return dtos;
        }

        //Mark a notification as read

        public async Task MarcarComoLeido(int notificacionId)
        {
            await _notificacionesRepository.MarcarComoLeido(notificacionId);
        }
    }
}

