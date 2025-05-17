using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IAdministradorService
    {
        Task<Administradores> AddAdministradoresAsync(AdministradorCreateDTO data);
        Task<bool> DeleteAdministradoresAsync(int id);
        Task<AdministradorListDTO> GetAdministradoresByIdAsync(int id);
        Task<IEnumerable<AdministradorListDTO>> GetAllAdministradoresAsync();
        Task<bool> UpdateAdministradoresAsync(AdministradorListDTO administradorListDTO);
    }
}