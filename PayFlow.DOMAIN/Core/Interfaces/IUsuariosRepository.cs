using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IUsuariosRepository
    {
        Task<int> AddUsuarioAsync(Usuarios usuario);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<IEnumerable<Usuarios>> GetAllUsuariosAsync();
        Task<Usuarios?> GetUsuarioByIdAsync(int id);
        Task<bool> UpdateUsuarioAsync(Usuarios usuario);
    }
}