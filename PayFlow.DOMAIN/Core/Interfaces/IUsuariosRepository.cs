using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<int> AddUsuarioAsync(Usuarios usuario);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<IEnumerable<Usuarios>> GetAllUsuariosAsync(string? filtro, string? busqueda, DateTime? fechaInicio, DateTime? fechaFin);
        Task<Usuarios?> GetUsuarioByIdAsync(int id);
        Task<bool> UpdateUsuarioAsync(Usuarios usuario);
        Task<Usuarios?> GetUsuarioByEmailAsync(string email);
        Task<bool> ResetPassword(string correo, string newPasswordHash);
        Task<Usuarios?> GetUsuarioByCorreoAsync(string Email);
        Task<Usuarios?> GetUsuarioByJwtTokenAsync(string jwtToken);
    }
}