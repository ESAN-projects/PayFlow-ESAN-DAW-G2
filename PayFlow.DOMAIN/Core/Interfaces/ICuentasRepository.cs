using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface ICuentasRepository
    {
        Task<Cuentas> GetCuentaByIdAsync(int cuentaId);
        Task<bool> UpdateCuentaAsync(Cuentas cuenta);
    }
}