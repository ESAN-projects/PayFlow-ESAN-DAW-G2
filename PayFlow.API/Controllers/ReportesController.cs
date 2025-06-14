using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Administrador")]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteFinancieroService _service;

        public ReportesController(IReporteFinancieroService service)
        {
            _service = service;
        }

        // Previsualización paginada del reporte
        [HttpPost("previsualizar")]
        public async Task<IActionResult> Previsualizar([FromBody] ReporteFinancieroFiltroDTO filtro)
        {
            var data = await _service.GenerarReporteAsync(filtro);
            return Ok(data.Take(50)); // Paginación simple (puedes agregar skip/take si usas frontend)
        }

        // Exportación a CSV
        [HttpPost("exportar/csv")]
        public async Task<IActionResult> ExportarCSV([FromBody] ReporteFinancieroFiltroDTO filtro)
        {
            var data = await _service.GenerarReporteAsync(filtro);
            var file = await _service.ExportarReporteCSVAsync(data);
            return File(file, "text/csv", "reporte_financiero.csv");
        }

        // Exportación a Excel
        [HttpPost("exportar/excel")]
        public async Task<IActionResult> ExportarExcel([FromBody] ReporteFinancieroFiltroDTO filtro)
        {
            var data = await _service.GenerarReporteAsync(filtro);
            var file = await _service.ExportarReporteExcelAsync(data);
            return File(file, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "reporte_financiero.xlsx");
        }
    }
}
