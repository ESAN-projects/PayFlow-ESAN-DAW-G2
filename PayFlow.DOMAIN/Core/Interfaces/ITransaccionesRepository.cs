using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface ITransaccionesRepository
    {
        Task<int> AddTransaccionAsync(Transacciones transaccion);
        Task<IEnumerable<Transacciones>> GetAllTransacciones();
        Task<Transacciones?> GetTransaccionById(int id);
        Task<IEnumerable<Transacciones>> GetTransaccionesByCuentaId(int cuentaId);
        Task<bool> RechazarTransaccion(int id);
        Task<bool> UpdateTransaccion(Transacciones transaccion);
        Task<int?> GetUltimoNumeroOperacionAsync();
        Task<IEnumerable<Transacciones>> GetTransaccionesByUsuario(int usuarioId, string? estado = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        Task<ResumenInicioDTO?> ObtenerResumenInicioAsync(int usuarioId);
        Task AddRangeTransaccionesAsync(IEnumerable<Transacciones> transacciones);
    }
}