using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Security.Claims;

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

        //Get all notificaciones
        [HttpGet]
        public async Task<IActionResult> GetAllNotificaciones()
        {
            var notificaciones = await _notificacionService.GetAllNotificaciones();
            return Ok(notificaciones);
        }
        //Get notificacion by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificacionById(int id)
        {
            var notificacion = await _notificacionService.GetNotificacionById(id);
            if (notificacion == null)
            {
                return NotFound();
            }
            return Ok(notificacion);
        }
        //Add notificacion
        [HttpPost]
        public async Task<IActionResult> AddNotificacion([FromBody] NotificacionCreateDTO data)
        {
            if (data == null)
            {
                return BadRequest();
            }
            var notificacionId = await _notificacionService.AddNotificacion(data);
            return CreatedAtAction(nameof(GetNotificacionById), new { id = notificacionId }, data);
        }

        //Update notificacion
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotificacion(int id, [FromBody] NotificacionDTO data)
        {
            if (id != data.NotificacionId)
            {
                return BadRequest();
            }
            var result = await _notificacionService.UpdateNotificacion(data);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        //Delete notificacion
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificacion(int id)
        {
            var result = await _notificacionService.DeleteNotificacion(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [Authorize]
        [HttpGet("no-leidas")]
        public async Task<IActionResult> ContarNoLeidas()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (usuarioIdClaim == null || !int.TryParse(usuarioIdClaim.Value, out int usuarioId))
                return Unauthorized("Usuario no identificado");

            // Obtener la cantidad de notificaciones no leídas
            int cantidad = await _notificacionService.ContarNotificacionesNoLeidas(usuarioId);

            return Ok(new { cantidad });
        }

        [Authorize]
        [HttpGet("usuario")]
        public async Task<IActionResult> ObtenerMisNotificaciones()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null || !int.TryParse(usuarioIdClaim.Value, out int usuarioId))
                return Unauthorized("Usuario no identificado");

            var notificaciones = await _notificacionService.ObtenerNotificacionesPorUsuario(usuarioId);

            return Ok(notificaciones);
        }

        [Authorize]
        [HttpPut("marcar-leida/{id}")]
        public async Task<IActionResult> MarcarComoLeida(int id)
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null || !int.TryParse(usuarioIdClaim.Value, out int usuarioId))
            {
                return Unauthorized("Usuario no identificado");
            }

            var resultado = await _notificacionService.MarcarNotificacionComoLeidaAsync(id, usuarioId);

            if (!resultado)
            {
                return NotFound("No se encontró la notificación o no pertenece al usuario.");
            }

            return NoContent(); // 204: Éxito sin contenido
        }

        [Authorize]
        [HttpPut("marcar-todas-como-leidas")]
        public async Task<IActionResult> MarcarTodasComoLeidas()
        {
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (usuarioIdClaim == null || !int.TryParse(usuarioIdClaim.Value, out int usuarioId))
                return Unauthorized("Usuario no identificado");

            var resultado = await _notificacionService.MarcarTodasComoLeidasAsync(usuarioId);
            if (!resultado)
                return BadRequest("No se pudieron actualizar las notificaciones.");

            return Ok(new { mensaje = "Todas las notificaciones han sido marcadas como leídas." });
        }


    }
}
