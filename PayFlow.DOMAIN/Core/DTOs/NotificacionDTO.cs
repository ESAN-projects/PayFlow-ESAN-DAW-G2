using System;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class NotificacionDTO
    {
        public int NotificacionID { get; set; }
        public int TransaccionID { get; set; }
        public string TipoTransaccion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaHora { get; set; }
        public string Mensaje { get; set; }
        public String Estado { get; set; }
    }
}
