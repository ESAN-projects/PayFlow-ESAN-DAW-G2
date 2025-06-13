using System;
using System.Collections.Generic;

namespace PayFlow.DOMAIN.Core.Entities;

public partial class Usuarios
{
    public int UsuarioId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string ContraseñaHash { get; set; } = null!;

    public string EstadoUsuario { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public virtual Cuentas? Cuentas { get; set; }

    public virtual ICollection<Notificacion> Notificaciones { get; set; } = new List<Notificacion>();
}
