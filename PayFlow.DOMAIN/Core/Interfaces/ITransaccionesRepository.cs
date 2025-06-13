using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface ITransaccionesRepository
    {
        Task<int> AddTransaccion(Transacciones transaccion);
        Task<IEnumerable<Transacciones>> GetAllTransacciones();
        Task<Transacciones?> GetTransaccionById(int id);
        Task<IEnumerable<Transacciones>> GetTransaccionesByCuentaId(int cuentaId);
        Task<bool> RechazarTransaccion(int id);
        Task<bool> UpdateTransaccion(Transacciones transaccion);
        Task<int?> GetUltimoNumeroOperacionAsync();
    }
}