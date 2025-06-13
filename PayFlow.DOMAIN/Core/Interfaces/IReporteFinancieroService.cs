using PayFlow.DOMAIN.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IReporteFinancieroService
    {
        Task<List<ReporteFinancieroItemDTO>> GenerarReporteAsync(ReporteFinancieroFiltroDTO filtro);
        Task<byte[]> ExportarReporteCSVAsync(List<ReporteFinancieroItemDTO> data);
        Task<byte[]> ExportarReporteExcelAsync(List<ReporteFinancieroItemDTO> data);
    }

}
