using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Infrastructure.Data;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.DTOs;

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
        [HttpGet]
        public async Task<IActionResult> GetAllAdministradores()
        {
            var administradores = await _administradorService.GetAllAdministradoresAsync();
            return Ok(administradores);
        }

        // GET Administrador by ID
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdministradores(int id, [FromBody] AdministradorListDTO administradorListDTO)
        {

            Console.WriteLine("Entro a Controlador");

            if (id != administradorListDTO.AdministradorId)
            {
                return BadRequest();
            }
            var updated = await _administradorService.UpdateAdministradoresAsync(administradorListDTO);
            if (updated == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Delete Administradores Logico
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
    }
}
