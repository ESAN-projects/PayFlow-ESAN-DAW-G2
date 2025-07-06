using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public TransaccionesController(ITransaccionesService transaccionesService)
        {
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
            Console.WriteLine("Ingresando a controlador");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int usuarioId))
            {
                Console.WriteLine("❌ Token no contiene el claim de ID de usuario.");
                return Unauthorized();
            }
            var transacciones = await _transaccionesService.GetTransaccionesByUsuario(usuarioId, estado, fechaInicio, fechaFin);
            return Ok(transacciones);
        }
    }
}
