using PayFlow.DOMAIN.Core.DTOs;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IAdministradoresService
    {
        Task<int> AddAdministradorAsync(AdministradoresCreateDTO adminCreateDTO);
        Task<bool> DeleteAdministradorAsync(int id);
        Task<AdministradoresListDTO?> GetAdministradorByIdAsync(int id);
        Task<IEnumerable<AdministradoresListDTO>> GetAllAdministradoresAsync();
        Task<bool> UpdateAdministradorAsync(AdministradoresUpdateDTO adminUpdateDTO);
    }
}