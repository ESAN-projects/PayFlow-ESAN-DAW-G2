using PayFlow.DOMAIN.Core.DTOs;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IUsuariosService
    {
        Task<int> AddUsuarioAsync(UsuariosCreateDTO usuarioCreateDTO);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<IEnumerable<UsuariosListDTO>> GetAllUsuariosAsync();
        Task<UsuariosListDTO?> GetUsuarioByIdAsync(int id);
        Task<bool> UpdateUsuarioAsync(UsuariosUpdateDTO usuarioUpdateDTO);
    }
}