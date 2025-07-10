using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IAdministradorService
    {
        Task<Administradores> AddAdministradoresAsync(AdministradorCreateDTO data);
        Task<bool> DeleteAdministradoresAsync(int id);
        Task<AdministradorListDTO> GetAdministradoresByIdAsync(int id);
        Task<IEnumerable<AdministradorListDTO>> GetAllAdministradoresAsync(string? filtro, string? busqueda);
        Task<bool> UpdateAdministradoresAsync(AdministradorListDTO administradorListDTO);
        Task<AuthAdmResponseDTO> LoginAsync(LoginAdmDTO loginAdmDTO);
        Task<string> ResetPasswordAsync(ResetPasswordAdmDTO resetPasswordAdmDTO);
        Task<bool> AceptarDepositoAsync(int transaccionId);
    }
}