using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class FiltroValidacionDTO
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Estado { get; set; } // "Aceptado", "Rechazado", "Pendiente"
        public int? ClienteId { get; set; }
        public string? NumeroComprobante { get; set; }
    }
}
