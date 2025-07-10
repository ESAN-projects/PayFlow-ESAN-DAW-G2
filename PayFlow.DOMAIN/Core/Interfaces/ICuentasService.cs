using PayFlow.DOMAIN.Core.DTOs;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface ICuentasService
    {
        Task<CuentasDTO?> GetCuentaById(int cuentaId);
        Task<CuentaUserDTO?> GetCuentaByUsuarioId(int usuarioId);
        Task<bool> UpdateCuenta(CuentasDTO cuentaDTO);
    }
}