using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificacionController : ControllerBase
    {
        private readonly INotificacionService _notificacionService;
        public NotificacionController(INotificacionService notificacionService)
        {
            _notificacionService = notificacionService;
        }


        //Get all notifications for a user
        [HttpGet("Usuario/{usuarioId}")]
        public async Task<IActionResult> ObtenerNotificacionesPorUsuario(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID de usuario inválido.");
            }

            var notificaciones = await _notificacionService.ObtenerNotificacionesPorUsuario(id);

            if (notificaciones == null || !notificaciones.Any())
            {
                return NotFound("No se encontraron notificaciones para este usuario.");
            }

            return Ok(notificaciones);
        }

        //Mark notification as read 
        [HttpPost("MarcarComoLeido/{notificacionId}")]
        public async Task<IActionResult> MarcarComoLeido(int notificacionId)
        {
            await _notificacionService.MarcarComoLeido(notificacionId);
            return NoContent(); // 204 No Content
        }

    }
}
