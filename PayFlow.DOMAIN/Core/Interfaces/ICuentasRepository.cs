using PayFlow.DOMAIN.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface ICuentasRepository
    {
        Task<Cuentas> GetCuentaByIdAsync(int cuentaId);
        Task UpdateCuentaAsync(Cuentas cuenta);
        Task<Cuentas?> ObtenerCuentaConTransaccionesAsync(int usuarioId);
        Task<Cuentas?> GetCuentaByUsuarioIdAsync(int usuarioId);
        Task<Cuentas?> ObtenerPorNumeroCuentaAsync(string numeroCuenta);
    }
}
