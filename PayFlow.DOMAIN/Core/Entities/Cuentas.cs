using System;
using System.Collections.Generic;

namespace PayFlow.DOMAIN.Core.Entities;

public partial class Cuentas
{
    public int CuentaId { get; set; }

    public int UsuarioId { get; set; }

    public string NumeroCuenta { get; set; } = null!;

    public decimal Saldo { get; set; }

    public string EstadoCuenta { get; set; } = null!;

    public virtual ICollection<Transacciones> TransaccionesCuenta { get; set; } = new List<Transacciones>();

    public virtual ICollection<Transacciones> TransaccionesCuentaDestino { get; set; } = new List<Transacciones>();

    public virtual Usuarios Usuario { get; set; } = null!;
}
