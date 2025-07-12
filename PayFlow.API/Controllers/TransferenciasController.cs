using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PayFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferenciasController : ControllerBase
    {
        private readonly ITransferenciaService _transferenciaService;
        private readonly ICuentasRepository _cuentasRepository;

        public TransferenciasController(ITransferenciaService transferenciaService, ICuentasRepository cuentasRepository)
        {
            _transferenciaService = transferenciaService;
            _cuentasRepository = cuentasRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TransferenciaRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int usuarioId))
                return Unauthorized("Usuario no identificado");

            var cuentaOrigen = await _cuentasRepository.GetCuentaByUsuarioId(usuarioId);
            if (cuentaOrigen == null)
                return BadRequest(new { Success = false, Message = "No se encontró la cuenta de origen para el usuario." });

            // Construir un nuevo DTO con la cuenta de origen
            var transferenciaRequest = new TransferenciaRequestDto
            {
                CuentaDestinoNumero = request.CuentaDestinoNumero,
                Monto = request.Monto
            };

            var result = await _transferenciaService.RealizarTransferenciaAsync(transferenciaRequest, cuentaOrigen.NumeroCuenta);
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
