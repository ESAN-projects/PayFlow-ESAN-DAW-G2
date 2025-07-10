using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IAdministradoresRepository
    {
        Task<Administradores> AddAdministradoresAsync(Administradores administradores);
        Task<bool> DeleteAdministradoresAsync(int id);
        Task<Administradores> GetAdministradoresByIdAsync(int id);
        Task<List<Administradores>> GetAllAdministradoresAsync(string? filtro, string? busqueda);
        Task<bool> RemoveAdministradoresAsync(int id);
        Task<bool> UpdateAdministradoresAsync(Administradores administrador);
        Task<Administradores?> GetAdministradorByEmailAsync(string email);
        Task<bool> ResetPassword(string correo, string newPassword);
    }
}