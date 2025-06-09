using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IAdministradoresRepository
    {
        Task<int> AddAdministradorAsync(Administradores administrador);
        Task<bool> DeleteAdministradorAsync(int id);
        Task<Administradores?> GetAdministradorByIdAsync(int id);
        Task<IEnumerable<Administradores>> GetAllAdministradoresAsync();
        Task<bool> UpdateAdministradorAsync(Administradores administrador);
    }
}