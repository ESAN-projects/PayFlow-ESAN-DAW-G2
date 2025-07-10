using PayFlow.DOMAIN.Core.DTOs;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface ICuentasService
    {
        Task<CuentaUsuarioDTO?> GetCuentaByUsuarioIdAsync(int usuarioId);
    }
}
