using PayFlow.DOMAIN.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IUsuarioDashboardService
    {
        Task<DashboardDTO> ObtenerDashboardAsync(int usuarioId);
    }
}
