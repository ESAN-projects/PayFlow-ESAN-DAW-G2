using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                TipoNotificacion = c.TipoNotificacion,
                Estado = c.Estado,

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

        public async Task<int> AddNotificacion(NotificacionCreateDTO data)
        {
            var notificacion = new Notificaciones
            {
                UsuarioId = data.UsuarioId,
                TransaccionId = data.TransaccionId,
                TipoNotificacion = data.TipoNotificacion,
                Mensaje = data.Mensaje,
                FechaHora = data.FechaHora,
                Estado = data.Estado,

            };
            return await _notificacionRepository.AddNotificacion(notificacion);

        }

        //Update notificacion
        public async Task<bool> UpdateNotificacion(NotificacionDTO data)
        {
            var notificacion = new Notificaciones
            {
                NotificacionId = data.NotificacionId,
                UsuarioId = data.UsuarioId,
                TransaccionId = data.TransaccionId,
                TipoNotificacion = data.TipoNotificacion,
                Mensaje = data.Mensaje,
                FechaHora = data.FechaHora,
                Estado = data.Estado,

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
