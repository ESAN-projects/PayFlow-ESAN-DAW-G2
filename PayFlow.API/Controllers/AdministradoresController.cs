using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Core.Servicies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PayFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministradoresController : ControllerBase
    {
        private readonly IAdministradoresService _administradoresService;
        public AdministradoresController(IAdministradoresService administradoresService)
        {
            _administradoresService = administradoresService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdministradoresListDTO>>> GetAll()
        {
            var result = await _administradoresService.GetAllAdministradoresAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdministradoresListDTO>> GetById(int id)
        {
            var result = await _administradoresService.GetAdministradorByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] AdministradoresCreateDTO dto)
        {
            var id = await _administradoresService.AddAdministradorAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] AdministradoresUpdateDTO dto)
        {
            var updated = await _administradoresService.UpdateAdministradorAsync(dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _administradoresService.DeleteAdministradorAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
