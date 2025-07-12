using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class RetiroDTO
    {
        public int TransaccionId { get; set; }
        public int CuentaId { get; set; }
        public decimal Monto { get; set; }
        public string? Iporigen { get; set; }
        public string Estado { get; set; } = null!;
    }
}
