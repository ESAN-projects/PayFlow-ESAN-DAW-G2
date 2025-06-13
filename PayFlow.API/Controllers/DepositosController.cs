using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositosController : ControllerBase
    {
        private readonly IDepositoService _depositosService;

        public DepositosController(IDepositoService depositosService)
        {
            _depositosService = depositosService;
        }

        // Endpoint para registrar el depósito
        [Authorize]
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarDeposito([FromForm] RegistrarDepositosDTO depositoDTO)
        {
            try
            {
                var deposito = await _depositosService.RegistrarDepositoAsync(depositoDTO);
                return Ok(new { Message = "Depósito registrado exitosamente", Deposito = deposito });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
