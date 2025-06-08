using System;
using System.Collections.Generic;

namespace PayFlow.DOMAIN.Core.Entities;

public partial class Notificacion
{
    public int NotificacionId { get; set; }

    public int UsuarioId { get; set; }

    public int? TransaccionId { get; set; }

    public string TipoNotificacion { get; set; } = null!;

    public string Mensaje { get; set; } = null!;

    public DateTime FechaHora { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Transacciones? Transaccion { get; set; }

    public virtual Usuarios Usuario { get; set; } = null!;
}
