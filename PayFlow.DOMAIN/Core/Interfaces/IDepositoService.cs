using PayFlow.DOMAIN.Core.DTOs;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IDepositoService
    {
        Task<DepositoDTO> RegistrarDepositoAsync(RegistrarDepositosDTO registrarDepositoDTO);
    }
}