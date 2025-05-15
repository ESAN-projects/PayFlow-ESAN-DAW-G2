using System;
using System.Collections.Generic;

namespace PayFlow.DOMAIN.Core.Entities;

public partial class Administradores
{
    public int AdministradorId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string CorreoElectronico { get; set; } = null!;

    public string ContraseñaHash { get; set; } = null!;

    public string EstadoAdministrador { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }

    public bool EsSuperAdmin { get; set; }

    public virtual ICollection<HistorialValidaciones> HistorialValidaciones { get; set; } = new List<HistorialValidaciones>();
}
