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

        public async Task<List<NotificacionxUsuarioDTO>> ObtenerNotificacionesPorUsuario(int usuarioId)
        {
            var entidades = await _notificacionesRepository.ObtenerNotificacionesPorUsuario(usuarioId);

            var dtos = new List<NotificacionxUsuarioDTO>();

            foreach (var entidad in entidades)
            {
                var dto = new NotificacionxUsuarioDTO
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

        public async Task<IEnumerable<NotificacionListDTO>> GetAllNotificaciones()
        {
            var notificaciones = await _notificacionesRepository.GetAllNotificaciones();
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
            var notificacion = await _notificacionesRepository.GetNotificacionById(id);
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
            var notificacion = new Notificacion
            {
                UsuarioId = data.UsuarioId,
                TransaccionId = data.TransaccionId,
                TipoNotificacion = data.TipoNotificacion,
                Mensaje = data.Mensaje,
                FechaHora = data.FechaHora,
                Estado = data.Estado,

            };
            return await _notificacionesRepository.AddNotificacion(notificacion);

        }

        //Update notificacion
        public async Task<bool> UpdateNotificacion(NotificacionDTO data)
        {
            var notificacion = new Notificacion
            {
                NotificacionId = data.NotificacionId,
                UsuarioId = data.UsuarioId,
                TransaccionId = data.TransaccionId,
                TipoNotificacion = data.TipoNotificacion,
                Mensaje = data.Mensaje,
                FechaHora = data.FechaHora,
                Estado = data.Estado,

            };
            return await _notificacionesRepository.UpdateNotificacion(notificacion);
        }


        //Delete notificacion
        public async Task<bool> DeleteNotificacion(int id)
        {
            var notificacion = await _notificacionesRepository.GetNotificacionById(id);
            if (notificacion == null)
            {
                return false;
            }
            return await _notificacionesRepository.DeleteNotificacion(notificacion.NotificacionId);
        }

        public async Task<int> ContarNotificacionesNoLeidas(int usuarioId)
        {
            return await _notificacionesRepository.ContarNuevas(usuarioId);
        }

        public async Task<bool> MarcarNotificacionComoLeidaAsync(int notificacionId, int usuarioId)
        {
            return await _notificacionesRepository.MarcarComoLeidaAsync(notificacionId, usuarioId);
        }

        public async Task<bool> MarcarTodasComoLeidasAsync(int usuarioId)
        {
            return await _notificacionesRepository.MarcarTodasComoLeidasAsync(usuarioId);
        }

    }
}

