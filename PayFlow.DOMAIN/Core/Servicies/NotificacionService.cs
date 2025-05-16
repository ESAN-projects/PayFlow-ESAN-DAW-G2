using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class NotificacionService : INotificacionService
    {
        private readonly INotificacionesRepository _notificacionRepository;

        public NotificacionService(INotificacionesRepository notificacionRepository)
        {
            _notificacionRepository = notificacionRepository;
        }

        public async Task<IEnumerable<NotificacionListDTO>> GetAllNotificaciones()
        {
            var notificaciones = await _notificacionRepository.GetAllNotificaciones();
            var notificacionesDTO = notificaciones.Select(c => new NotificacionListDTO
            {
                NotificacionId = c.NotificacionId,
                TransaccionId = c.TransaccionId,
                UsuarioId = c.UsuarioId,
                FechaHora = c.FechaHora,
                Mensaje = c.Mensaje,
                TipoNotificacion = c.TipoNotificacion

            });

            return notificacionesDTO;
        }

        public async Task<NotificacionListDTO> GetNotificacionById(int id)
        {
            var notificacion = await _notificacionRepository.GetNotificacionById(id);
            if (notificacion == null)
            {
                return null;
            }
            var categoryDTO = new NotificacionListDTO
            {
                NotificacionId = notificacion.NotificacionId,
                TransaccionId = notificacion.TransaccionId,
                UsuarioId = notificacion.UsuarioId,
                FechaHora = notificacion.FechaHora,
                Mensaje = notificacion.Mensaje,
                TipoNotificacion = notificacion.TipoNotificacion
            };
            return categoryDTO;
        }

        public async Task<int> AddNotificacion(NotificacionCreateDTO notificacionDTO)
        {
            var notificacion = new Notificaciones
            {
                TipoNotificacion = notificacionDTO.TipoNotificacion,
                Mensaje = notificacionDTO.Mensaje,
                FechaHora = notificacionDTO.FechaHora,
                Estado = notificacionDTO.Estado,

            };
            return await _notificacionRepository.AddNotificacion(notificacion);

        }

        //Update notificacion
        public async Task<bool> UpdateNotificacion(NotificacionListDTO dato)
        {
            var notificacion = new Notificaciones
            {
                NotificacionId = dato.NotificacionId,
                UsuarioId = dato.UsuarioId,
                TransaccionId = dato.TransaccionId,
                TipoNotificacion = dato.TipoNotificacion,
                Mensaje = dato.Mensaje,
                FechaHora = dato.FechaHora,

            };
            return await _notificacionRepository.UpdateNotificacion(notificacion);
        }


        //Delete notificacion
        public async Task<bool> DeleteNotificacion(int id)
        {
            var notificacion = await _notificacionRepository.GetNotificacionById(id);
            if (notificacion == null)
            {
                return false;
            }
            return await _notificacionRepository.DeleteNotificacion(notificacion.NotificacionId);
        }

    }
}
