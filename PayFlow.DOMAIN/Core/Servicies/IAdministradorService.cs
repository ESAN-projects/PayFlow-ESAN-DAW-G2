using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public interface IAdministradorService
    {
        Task<Administradores> AddAdministradoresAsync(AdministradorCreateDTO administradorCreateDTO);
        Task<bool> Delete2AdministradoresAsync(int id);
        Task<bool> DeleteAdministradoresAsync(int id);
        Task<AdministradorListDTO> GetAdministradoresByIdAsync(int id);
        Task<IEnumerable<AdministradorListDTO>> GetAllAdministradoresAsync();
        Task<Administradores> UpdateAdministradoresAsync(AdministradorListDTO administradorListDTO);
    }
}