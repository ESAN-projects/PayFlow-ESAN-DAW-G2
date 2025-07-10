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
        // Nuevo método para filtrar por usuario, estado y fechas
        Task<IEnumerable<TransaccionesListDTO>> GetTransaccionesByUsuario(int usuarioId, string? estado = null, DateTime? fechaInicio = null, DateTime? fechaFin = null);
        
        //Nuevo método para página de inicio
        Task<ResumenInicioDTO?> ObtenerResumenInicioAsync(int usuarioId);

    }
}