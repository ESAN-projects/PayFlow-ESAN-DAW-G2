using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Administrador")] 
    public class ValidacionManualController : ControllerBase
    {
        private readonly IValidacionManualService _validacionService;

        public ValidacionManualController(IValidacionManualService validacionService)
        {
            _validacionService = validacionService;
        }

        // POST: api/ValidacionManual/validar
        [HttpPost("validar")]
        public async Task<IActionResult> Validar([FromBody] ValidacionManualDTO dto)
        {
            try
            {
                await _validacionService.ValidarManualAsync(dto);
                return Ok(new { mensaje = "Validación registrada exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/ValidacionManual/historial
        [HttpPost("historial")]
        public async Task<IActionResult> Historial([FromBody] FiltroValidacionDTO filtro)
        {
            var historial = await _validacionService.ObtenerHistorialAsync(filtro);
            return Ok(historial);
        }
    }
}
