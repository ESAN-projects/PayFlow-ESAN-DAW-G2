using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;
        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        // get all usuarios con filtros
        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios(
            [FromQuery] string? filtro,
            [FromQuery] string? busqueda,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            var usuarios = await _usuariosService.GetAllUsuariosAsync(filtro, busqueda, fechaInicio, fechaFin);
            return Ok(usuarios);
        }

        //get by id usuarios
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioById(int id)
        {
            var usuario = await _usuariosService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }

        //add usuarios
        [HttpPost]
        public async Task<IActionResult> AddUsuario([FromBody] UsuariosCreateDTO usuarioCreateDTO)
        {
            if (usuarioCreateDTO == null)
            {
                return BadRequest();
            }
            var id = await _usuariosService.AddUsuarioAsync(usuarioCreateDTO);
            if (id == 0)
            {
                return Conflict(new { message = "El correo electrónico ya está registrado." });
            }
            return CreatedAtAction(nameof(GetUsuarioById), new { id }, new { Id = id });
        }

        //update usuarios
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuariosUpdateDTO usuarioUpdateDTO)
        {
            if (id != usuarioUpdateDTO.UsuarioId)
            {
                return BadRequest();
            }
            var result = await _usuariosService.UpdateUsuarioAsync(usuarioUpdateDTO);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        //delete usuarios
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var result = await _usuariosService.DeleteUsuarioAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        //login usuarios
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            if (loginDTO == null)
            {
                return BadRequest();
            }
            var authResponse = await _usuariosService.LoginAsync(loginDTO);
            if (authResponse == null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);
        }

        //Reset password usuarios
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            if (resetPasswordDTO == null)
            {
                return BadRequest();
            }
            var result = await _usuariosService.ResetPasswordAsync(resetPasswordDTO);
            if (result == "Usuario no encontrado o inactivo.")
            {
                return NotFound(new { message = result });
            }
            if (result == "Error al restablecer la contraseña.")
            {
                return BadRequest(new { message = result });
            }
            return Ok(new { message = result });
        }

        //Obtener usuario logueado por JWT
        [Authorize]
        [HttpGet("usuarioByjwt")]
        public async Task<IActionResult> GetUsuarioByJwt()
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized();
            }
            var jwtToken = authHeader.Substring("Bearer ".Length).Trim();
            var usuario = await _usuariosService.GetUsuarioByJwtTokenAsync(jwtToken);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado o inactivo." });
            }
            return Ok(usuario);
        }
    }
}
