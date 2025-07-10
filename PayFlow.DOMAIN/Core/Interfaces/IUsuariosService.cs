using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IUsuariosService
    {
        Task<int> AddUsuarioAsync(UsuariosCreateDTO usuarioCreateDTO);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<IEnumerable<UsuariosListDTO>> GetAllUsuariosAsync(string? filtro, string? busqueda, DateTime? fechaInicio, DateTime? fechaFin);
        Task<UsuariosListDTO?> GetUsuarioByIdAsync(int id);
        Task<bool> UpdateUsuarioAsync(UsuariosUpdateDTO usuarioUpdateDTO);
        Task<bool> ActualizarPerfilAsync(int usuarioId, PerfilUpdateDTO dto);
        
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDTO);
        Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO);
        Task<UsuariosListDTO?> GetUsuarioByJwtTokenAsync(string jwtToken);
    }
}