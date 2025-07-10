using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class CuentasDTO
    {
        public int CuentaId { get; set; }
        public int UsuarioId { get; set; }
        public string NumeroCuenta { get; set; } = null!;
        public decimal Saldo { get; set; }
        public string EstadoCuenta { get; set; } = null!;
    }
    public class CuentaUserDTO
    {
        public int CuentaId { get; set; }
        public int UsuarioId { get; set; }
        public decimal Saldo { get; set; }
        public string NumeroCuenta { get; set; } = null!;
    }
}
