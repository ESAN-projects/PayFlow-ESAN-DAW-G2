using PayFlow.DOMAIN.Core.DTOs;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IRetiroService
    {
        Task<RetiroDTO?> GetRetiroById(int transactionId);
        Task<int> AddRetiro(RetiroCreateDTO retiroCreateDTO, string Iporigen);
    }
}