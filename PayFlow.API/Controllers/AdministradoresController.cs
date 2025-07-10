using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        private readonly IAdministradorService _administradorService;
        public AdministradoresController(IAdministradorService administradorService)
        {
            _administradorService = administradorService;
        }

        // GET all Administradores
        // [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllAdministradores([FromQuery] string? filtro, [FromQuery] string? busqueda)
        {
            var administradores = await _administradorService.GetAllAdministradoresAsync(filtro, busqueda);
            return Ok(administradores);
        }

        // GET Administrador by ID
        // [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdministradoresById(int id)
        {
            var administradores = await _administradorService.GetAdministradoresByIdAsync(id);
            if (administradores == null)
            {
                return NotFound();
            }
            return Ok(administradores);
        }
        // Add Administradores
        // [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAdministradores([FromBody] AdministradorCreateDTO administradorCreateDTO)
        {
            if (administradorCreateDTO == null)
            {
                return BadRequest();
            }
            var AdministradoresId = await _administradorService.AddAdministradoresAsync(administradorCreateDTO);
            return CreatedAtAction(nameof(GetAdministradoresById), new { id = AdministradoresId }, administradorCreateDTO);
        }

        // Update Administradores
        //Check it out
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdministradores(int id, [FromBody] AdministradorListDTO administradorListDTO)
        {

            Console.WriteLine("Entro a Controlador");

            if (id != administradorListDTO.AdministradorId)
            {
                return BadRequest();
            }
            var updated = await _administradorService.UpdateAdministradoresAsync(administradorListDTO);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Delete Administradores Logico
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdministradores(int id)
        {
            var deleted = await _administradorService.DeleteAdministradoresAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Login administrador
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAdmDTO loginAmdDTO)
        {
            if (loginAmdDTO == null)
            {
                return BadRequest();
            }
            var authResponse = await _administradorService.LoginAsync(loginAmdDTO);
            if (authResponse == null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);
        }

        // Restablecer contraseña administrador
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordAdmDTO resetPasswordAdmDTO)
        {
            if (resetPasswordAdmDTO == null)
            {
                return BadRequest();
            }
            var result = await _administradorService.ResetPasswordAsync(resetPasswordAdmDTO);
            if (result == "Administrador no encontrado o inactivo.")
            {
                return NotFound();
            }
            return NoContent();
        }

        // Endpoint para obtener aporbar deposito por un administrador autenticado
        [Authorize]
        [HttpPost("aceptar-deposito/{transaccionId}")]
        public async Task<IActionResult> AceptarDeposito(int transaccionId)
        {
            try
            {
                var result = await _administradorService.AceptarDepositoAsync(transaccionId);
                if (result)
                {
                    return Ok(new { Message = "Depósito aceptado y saldo actualizado." });
                }
                return BadRequest(new { Message = "Error al aceptar el depósito." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
