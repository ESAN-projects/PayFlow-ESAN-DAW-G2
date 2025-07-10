using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Threading.Tasks;

namespace PayFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentasService _cuentasService;
        public CuentasController(ICuentasService cuentasService)
        {
            _cuentasService = cuentasService;
        }

        // GET: api/cuentas/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetCuentaByUsuarioId(int usuarioId)
        {
            var cuenta = await _cuentasService.GetCuentaByUsuarioId(usuarioId);
            if (cuenta == null)
                return NotFound();
            return Ok(cuenta);
        }
    }
}
