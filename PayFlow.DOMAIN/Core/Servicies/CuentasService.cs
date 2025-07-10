using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class CuentasService : ICuentasService
    {
        private readonly ICuentasRepository _cuentasRepository;
        public CuentasService(ICuentasRepository cuentasRepository)
        {
            _cuentasRepository = cuentasRepository;
        }

        public async Task<CuentaUsuarioDTO?> GetCuentaByUsuarioIdAsync(int usuarioId)
        {
            var cuenta = await _cuentasRepository.ObtenerCuentaConTransaccionesAsync(usuarioId);
            if (cuenta == null)
                return null;
            return new CuentaUsuarioDTO
            {
                CuentaId = cuenta.CuentaId,
                NumeroCuenta = cuenta.NumeroCuenta,
                Saldo = cuenta.Saldo,
                EstadoCuenta = cuenta.EstadoCuenta
            };
        }
    }
}
