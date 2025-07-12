using System.ComponentModel.DataAnnotations;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class TransferenciaRequestDto
    {
        [Required(ErrorMessage = "El número de cuenta de destino es requerido.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El número de cuenta debe tener 10 dígitos.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "El número de cuenta debe contener solo dígitos.")]
        public string? CuentaDestinoNumero { get; set; }

        [Required(ErrorMessage = "El monto es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal Monto { get; set; }
    }
}
