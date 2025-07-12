using System.ComponentModel.DataAnnotations;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class RetiroCreateDTO
    {
        [Required(ErrorMessage = "El monto es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero.")]
        public decimal Monto { get; set; }
    }
}
