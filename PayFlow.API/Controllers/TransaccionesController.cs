using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Security.Claims;


namespace PayFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransaccionesController : ControllerBase
    {
        private readonly ITransaccionesService _transaccionesService;
        private readonly ILogger<TransaccionesController> _logger;
        public TransaccionesController(ILogger<TransaccionesController> logger, ITransaccionesService transaccionesService)
        {
            _logger = logger;
            _transaccionesService = transaccionesService;
        }
        //Get all transacciones
        [HttpGet]
        public async Task<IActionResult> GetAllTransacciones()
        {
            var transacciones = await _transaccionesService.GetAllTransacciones();
            return Ok(transacciones);
        }
        //Get transacciones by id
        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var transaccion = await _transaccionesService.GetTransaccionById(transactionId);
            if (transaccion == null)
            {
                return NotFound();
            }
            return Ok(transaccion);
        }

        //Add transacciones
        [HttpPost]
        public async Task<IActionResult> AddTransaccion([FromBody] TransaccionesCreateDTO transaccion)
        {
            if(transaccion == null)
            {
                return BadRequest();
            }
            var transaccionId = await _transaccionesService.AddTransaccion(transaccion);
            return CreatedAtAction(nameof(GetTransactionById), new { transactionId = transaccionId }, transaccion);
        }
        //Update transacciones
        [HttpPut("{transactionId}")]
        public async Task<IActionResult> UpdateTransaccion(int transactionId, [FromBody] TransaccionesCreateDTO transaccion)
        {
            if (transactionId == 0)
            {
                return BadRequest();
            }
            var result = await _transaccionesService.UpdateTransaccion(transaccion);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        //Rechazar transacciones
        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> RechazarTransaccion(int transactionId)
        {
            var result = await _transaccionesService.RechazarTransaccion(transactionId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        //Get transacciones by cuentaId
        [HttpGet("cuenta/{cuentaId}")]
        public async Task<IActionResult> GetTransaccionesByCuentaId(int cuentaId)
        {
            var transaccion = await _transaccionesService.GetTransaccionesByCuentaId(cuentaId);
            if (transaccion == null)
            {
                return NotFound();
            }
            return Ok(transaccion);
        }
        //Get transacciones by usuario, estado y fechas
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetTransaccionesByUsuario(int usuarioId, [FromQuery] string? estado = null, [FromQuery] DateTime? fechaInicio = null, [FromQuery] DateTime? fechaFin = null)
        {
            var transacciones = await _transaccionesService.GetTransaccionesByUsuario(usuarioId, estado, fechaInicio, fechaFin);
            if (transacciones == null)
            {
                return NotFound();
            }
            return Ok(transacciones);
        }


        [Authorize]
        [HttpGet("mis-transacciones")]
        public async Task<IActionResult> GetMisTransacciones([FromQuery] string? estado = null, [FromQuery] DateTime? fechaInicio = null, [FromQuery] DateTime? fechaFin = null)
        {
            _logger.LogInformation("📥 Ingresando a 'GetMisTransacciones'");
            _logger.LogInformation("🔎 Filtros recibidos → Estado: {Estado}, FechaInicio: {FechaInicio}, FechaFin: {FechaFin}", estado, fechaInicio, fechaFin);


            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int usuarioId))
            {
                _logger.LogWarning("❌ Token no contiene el claim de ID de usuario.");
                return Unauthorized();
            }
            var transacciones = await _transaccionesService.GetTransaccionesByUsuario(usuarioId, estado, fechaInicio, fechaFin);
            return Ok(transacciones);
        }

        [Authorize]
        [HttpGet("resumen-inicio")]
        public async Task<IActionResult> ObtenerResumenInicio()
        {

            _logger.LogInformation("Ingresando a resumen Inicio");
            // Extraer usuarioId desde el token JWT
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (usuarioIdClaim == null)
                return Unauthorized("Usuario no identificado");

            if (!int.TryParse(usuarioIdClaim.Value, out int usuarioId))
                return Unauthorized("Usuario inválido");

            var resumen = await _transaccionesService.ObtenerResumenInicioAsync(usuarioId);

            if (resumen == null)
                return NotFound("No se encontró información para este usuario.");

            return Ok(resumen);
        }
    }
}
