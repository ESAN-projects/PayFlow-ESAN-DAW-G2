using Microsoft.AspNetCore.Http;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class RegistrarDepositosDTO
    {
            public decimal Monto { get; set; }
            public string Banco { get; set; }
            public IFormFile RutaVoucher { get; set; }
            public string? ComentariosAdmin { get; set; }
            public string? Comentario { get; set; }
            public int? CuentaDestinoID { get; set; }
            public string? IPOrigen { get; set; }
            public string? Ubicacion { get; set; }
    }

    public class DepositoDTO
    {
        public decimal Monto { get; set; }
        public string NumeroOperacion { get; set; }
        //public string RutaVoucher { get; set; }
        public DateTime FechaTransaccion { get; set; }

        public int TransaccionId { get; set; }
    }
}
