using System;
using System.Collections.Generic;

namespace PayFlow.DOMAIN.Core.Entities;

public partial class HistorialValidaciones
{
    public int ValidacionId { get; set; }

    public int TransaccionId { get; set; }

    public int AdministradorId { get; set; }

    public DateTime FechaHora { get; set; }

    public string TipoValidacion { get; set; } = null!;

    public string Resultado { get; set; } = null!;

    public string? Comentarios { get; set; }

    public virtual Administradores Administrador { get; set; } = null!;

    public  Transacciones Transaccion { get; set; } = null!;
    // public virtual Transacciones Transaccion { get; set; } = null!;
}
