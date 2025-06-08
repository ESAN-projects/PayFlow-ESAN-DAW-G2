using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;

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

        // GET: api/dashboard/{usuarioId}
        [HttpGet("{usuarioId}")]
        public async Task<ActionResult<DashboardDTO>> GetDashboard(int usuarioId)
        {
            try
            {
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
