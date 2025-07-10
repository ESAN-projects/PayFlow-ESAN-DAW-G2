namespace PayFlow.DOMAIN.Core.DTOs
{
    public class TransaccionesDTO
    {
        public int TransaccionId { get; set; }
        public int CuentaId { get; set; }
        public string TipoTransaccion { get; set; } = null!;
        public decimal Monto { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;
        public string? NumeroOperacion { get; set; }
        public string? Banco { get; set; }
        public string? RutaVoucher { get; set; }
        public string? ComentariosAdmin { get; set; }
        public string? Comentario { get; set; }
        public int? CuentaDestinoId { get; set; }
        public string? Iporigen { get; set; }
        public string? Ubicacion { get; set; }
    }
    public class TransaccionesCreateDTO
    {
        public int TransaccionId { get; set; }
        public int CuentaId { get; set; }
        public string TipoTransaccion { get; set; } = null!;
        public decimal Monto { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;
    }

    public class TransaccionesListDTO
    {
        public int TransaccionId { get; set; }
        public int CuentaId { get; set; }
        public string TipoTransaccion { get; set; } = null!;
        public decimal Monto { get; set; }
        public DateTime FechaHora { get; set; }
        public string Estado { get; set; } = null!;
        public string? Iporigen { get; set; }
        public string? Ubicacion { get; set; }
    }

    public class TransaccionResumenDTO
    {
        public DateTime Fecha { get; set; }
        public string TipoTransaccion { get; set; }  // "Depósito", "Retiro", "Transferencia"
        public decimal Monto { get; set; }
        public string Estado { get; set; }
    }

    public class ResumenInicioDTO
    {
        public string NumeroCuenta { get; set; }
        public decimal Saldo { get; set; }

        public List<MovimientoDTO> Movimientos { get; set; } = new();
    }

    public class MovimientoDTO
    {
        public string TipoTransaccion { get; set; }
        public string NumeroOperacion { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
    }

}
