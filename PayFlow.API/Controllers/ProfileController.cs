using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Core.DTOs;

namespace PayFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;

        public ProfileController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }
        [HttpPut("actualizar")]
        
        public async Task<IActionResult> ActualizarPerfil([FromBody] PerfilUpdateDTO dto)
        {
            int usuarioId = dto.UsuarioId;

            bool actualizado = await _usuariosService.ActualizarPerfilAsync(usuarioId, dto);
            if (!actualizado)
                return NotFound("Usuario no encontrado.");

            return NoContent(); // 204
        }

    }
}
