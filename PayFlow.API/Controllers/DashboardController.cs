using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Security.Claims;

namespace PayFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IUsuarioDashboardService _dashboardService;

        public DashboardController(IUsuarioDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Obtener el ID del usuario autenticado desde el token JWT (claim NameIdentifier)
        [HttpGet]
        public async Task<ActionResult<DashboardDTO>> GetDashboard()
        {
            try
            {
                // Obtener el ID del usuario autenticado desde el token
                var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (usuarioIdClaim == null)
                    return Unauthorized(new { mensaje = "No se pudo identificar al usuario." });

                int usuarioId = int.Parse(usuarioIdClaim.Value);

                var dashboard = await _dashboardService.ObtenerDashboardAsync(usuarioId);
                return Ok(dashboard);
            }
            catch (Exception ex)
            {
                return NotFound(new { mensaje = ex.Message });
            }
        }

    }
}
