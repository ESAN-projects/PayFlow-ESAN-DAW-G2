using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Infrastructure.Data;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        private readonly IAdministradoresRepository _administradoresRepository;

        public AdministradoresController(IAdministradoresRepository administradoresRepository)
        {
            _administradoresRepository = administradoresRepository;
        }

        // GET all Administradores
        [HttpGet]
        public async Task<IActionResult> GetAllAdministradores()
        {
            var administradores = await _administradoresRepository.GetAllAdministradoresAsync();    
            return Ok(administradores);
        }

        // GET Administrador by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdministradoresById(int id)
        {
            var administradores = await _administradoresRepository.GetAdministradoresByIdAsync(id);
            if (administradores == null)
            {
                return NotFound();
            }
            return Ok(administradores);
        }
        // POST add Administradores
        [HttpPost]
        public async Task<IActionResult> AddAdministradores([FromBody] Administradores administradores)
        {
            if (administradores == null)
            {
                return BadRequest();
            }
            var AdministradoresId = await _administradoresRepository.AddAdministradoresAsync(administradores);
            return CreatedAtAction(nameof(GetAdministradoresById), new { id = AdministradoresId }, administradores);
        }
    }
}
