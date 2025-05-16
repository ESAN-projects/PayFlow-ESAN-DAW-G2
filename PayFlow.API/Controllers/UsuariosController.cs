using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosService _usuariosService;
        public UsuariosController(IUsuariosService usuariosService)
        {
            _usuariosService = usuariosService;
        }

        // get all usuarios
        [HttpGet]
        public async Task<IActionResult> GetAllUsuarios()
        {
            var usuarios = await _usuariosService.GetAllUsuariosAsync();
            return Ok(usuarios);
        }

        //get by id usuarios
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
            return CreatedAtAction(nameof(GetUsuarioById), new { id }, new { Id = id });
        }

        //update usuarios
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
    }
}
