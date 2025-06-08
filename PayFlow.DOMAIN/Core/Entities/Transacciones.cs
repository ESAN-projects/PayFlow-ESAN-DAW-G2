using System;
using System.Collections.Generic;

namespace PayFlow.DOMAIN.Core.Entities;

public partial class Transacciones
{
    public int TransaccionId { get; set; }

    public int CuentaId { get; set; }

    public string TipoTransaccion { get; set; } = null!;

    public decimal Monto { get; set; }

    public DateTime FechaHora { get; set; }

    public string Estado { get; set; } = null!;

    public string? NumeroOperacion { get; set; }

    public string? Banco { get; set; }

    public string? RutaVoucher { get; set; }

    public string? ComentariosAdmin { get; set; }

    public string? Comentario { get; set; }

    public int? CuentaDestinoId { get; set; }

    public string? Iporigen { get; set; }

    public string? Ubicacion { get; set; }

    public virtual Cuentas Cuenta { get; set; } = null!;

    public virtual Cuentas? CuentaDestino { get; set; }

    public virtual ICollection<HistorialValidaciones> HistorialValidaciones { get; set; } = new List<HistorialValidaciones>();

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
}
