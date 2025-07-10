namespace PayFlow.DOMAIN.Core.DTOs
{
    public class CuentaUsuarioDTO
    {
        public int CuentaId { get; set; }
        public string NumeroCuenta { get; set; } = null!;
        public decimal Saldo { get; set; }
        public string EstadoCuenta { get; set; } = null!;
    }
}