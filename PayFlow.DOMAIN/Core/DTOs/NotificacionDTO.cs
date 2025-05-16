using PayFlow.DOMAIN.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class NotificacionDTO
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public int? TransaccionId { get; set; }
        public string TipoNotificacion { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;
        public virtual Transacciones? Transaccion { get; set; }
        public virtual Usuarios Usuario { get; set; } = null!;

    }

    public class NotificacionListDTO
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public int? TransaccionId { get; set; }
        public string TipoNotificacion { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public DateTime FechaHora { get; set; }
    }

    public class NotificacionCreateDTO
    {
        public string? TipoNotificacion { get; set; }
        public string Mensaje { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;
    }

}
