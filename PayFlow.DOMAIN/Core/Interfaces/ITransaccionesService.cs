using PayFlow.DOMAIN.Core.DTOs;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface ITransaccionesService
    {
        Task<int> AddTransaccion(TransaccionesCreateDTO transaccionDTO);
        Task<IEnumerable<TransaccionesListDTO>> GetAllTransacciones();
        Task<IEnumerable<TransaccionesListDTO>> GetTransaccionesByCuentaId(int cuentaId);
        Task<TransaccionesDTO> GetTransaccionById(int transactionId);
        Task<bool> RechazarTransaccion(int transaccionId);
        Task<bool> UpdateTransaccion(TransaccionesCreateDTO transaccionDTO);
    }
}