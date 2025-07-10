namespace PayFlow.DOMAIN.Core.DTOs
{
    public class TransferenciaResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? NumeroOperacion { get; set; }
        public decimal? MontoTransferido { get; set; }
    }
}
