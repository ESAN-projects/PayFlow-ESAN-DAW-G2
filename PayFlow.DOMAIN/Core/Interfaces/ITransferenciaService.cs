using PayFlow.DOMAIN.Core.DTOs;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface ITransferenciaService
    {
        Task<TransferenciaResponseDto> RealizarTransferenciaAsync(TransferenciaRequestDto requestDto, string cuentaOrigenNumero);
    }
}
