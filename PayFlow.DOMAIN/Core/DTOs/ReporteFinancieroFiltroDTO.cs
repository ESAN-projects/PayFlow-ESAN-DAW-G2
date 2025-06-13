using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class ReporteFinancieroFiltroDTO
    {
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? TipoTransaccion { get; set; } // "Depósito", "Retiro"
        public int? UsuarioId { get; set; }
        public string? Estado { get; set; } // "Aceptado", "Rechazado", "Pendiente"
    }

}
