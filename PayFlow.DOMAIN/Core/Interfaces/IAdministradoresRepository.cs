using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IAdministradoresRepository
    {
        Task<Administradores> AddAdministradoresAsync(Administradores administradores);
        Task<bool> DeleteAdministradoresAsync(int id);
        Task<Administradores> GetAdministradoresByIdAsync(int id);
        Task<List<Administradores>> GetAllAdministradoresAsync();
        Task<bool> RemoveAdministradoresAsync(int id);
        Task<bool> UpdateAdministradoresAsync(Administradores administrador);
    }
}