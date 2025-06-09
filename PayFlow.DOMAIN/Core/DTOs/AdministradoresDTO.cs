using System;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class AdministradoresListDTO
    {
        public int AdministradorId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string EstadoAdministrador { get; set; }
        public bool EsSuperAdmin { get; set; }
    }

    public class AdministradoresCreateDTO
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string ContraseñaHash { get; set; }
        public string EstadoAdministrador { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool EsSuperAdmin { get; set; }
    }

    public class AdministradoresUpdateDTO
    {
        public int AdministradorId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string ContraseñaHash { get; set; }
        public string EstadoAdministrador { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool EsSuperAdmin { get; set; }
    }
}
