using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class ReporteFinancieroItemDTO
    {
        public int TransaccionId { get; set; }
        public string NombreUsuario { get; set; }
        public string TipoTransaccion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; }
        public string IpOrigen { get; set; }
        public string? Ubicacion { get; set; } // opcional
        public string? Comentarios { get; set; }
        public bool EsSospechosa { get; set; }
    }

}
