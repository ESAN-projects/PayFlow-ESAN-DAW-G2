using System;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class NotificacionxUsuarioDTO
    {
        public int NotificacionID { get; set; }
        public int TransaccionID { get; set; }
        public string TipoTransaccion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaHora { get; set; }
        public string Mensaje { get; set; }
        public String Estado { get; set; }
    }

    public class NotificacionDTO
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public int? TransaccionId { get; set; }
        public string TipoNotificacion { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;

    }

    public class NotificacionListDTO
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public int? TransaccionId { get; set; }
        public string TipoNotificacion { get; set; } = null!;
        public string Mensaje { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;
    }

    public class NotificacionCreateDTO
    {
        public int UsuarioId { get; set; }
        public int? TransaccionId { get; set; }
        public string? TipoNotificacion { get; set; }
        public string Mensaje { get; set; } = null!;
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;
    }

}
