using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class DashboardDTO
    {
        public decimal SaldoActual { get; set; }
        public List<TransaccionResumenDTO> UltimasTransacciones { get; set; } = new();

    }
}
