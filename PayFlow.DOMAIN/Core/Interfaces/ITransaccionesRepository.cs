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
        // Nuevo método para filtrar por usuario, estado y fechas
        Task<IEnumerable<Transacciones>> GetTransaccionesByUsuario(int usuarioId, string? estado = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
    }
}