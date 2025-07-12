using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Security.Claims;


namespace PayFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RetirosController : ControllerBase
    {
        private readonly IRetiroService _retiroService;
        public RetirosController(IRetiroService transaccionesService)
        {
            _retiroService = transaccionesService;
        }

        //Get retiro by id
        [Authorize]
        [HttpGet("{retiroId}")]
        public async Task<IActionResult> GetRetiroById(int retiroId)
        {
            var transaccion = await _retiroService.GetRetiroById(retiroId);
            if (transaccion == null)
            {
                return NotFound();
            }
            return Ok(transaccion);
        }
        //Add retiro
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddRetiro([FromBody] RetiroCreateDTO retiro)
        {
            if(retiro == null)
            {
                return BadRequest();
            }
            var Iporigen = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int usuarioId))
                {
                   throw new ArgumentException("Usuario no encontrado o inválido.");
                }

                var transaccionId = await _retiroService.AddRetiro(retiro, Iporigen);
                if (transaccionId <= 0)
                {
                    return BadRequest("Error al procesar el retiro.");
                }

                var retiroCreated = await _retiroService.GetRetiroById(transaccionId);
                return CreatedAtAction(nameof(GetRetiroById), new { retiroId = transaccionId }, retiroCreated);
            }
            catch (Exception ex)
            {
                // Retornar el mensaje de excepción directamente para depuración
                return BadRequest(ex.Message);
            }
        }
    }
}
