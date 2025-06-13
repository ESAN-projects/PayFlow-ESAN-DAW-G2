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
<<<<<<< HEAD
        Task<bool> ActualizarPerfilAsync(int usuarioId, PerfilUpdateDTO dto);
        
=======
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
        Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
>>>>>>> bb7536d3585a20b6fc434edcd3b09fcf90c48232
    }
}