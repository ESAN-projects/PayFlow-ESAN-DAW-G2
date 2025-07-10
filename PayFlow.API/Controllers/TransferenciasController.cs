using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Threading.Tasks;

namespace PayFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciasController : ControllerBase
    {
        private readonly ITransferenciaService _transferenciaService;

        public TransferenciasController(ITransferenciaService transferenciaService)
        {
            _transferenciaService = transferenciaService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransferenciaRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _transferenciaService.RealizarTransferenciaAsync(request);
            if (!result.Success)
            {
                if (result.Message.Contains("no existe") || result.Message.Contains("no está activa") || result.Message.Contains("Saldo insuficiente") || result.Message.Contains("misma"))
                    return BadRequest(result);
                return StatusCode(500, result);
            }
            return Ok(result);
        }
    }
}
