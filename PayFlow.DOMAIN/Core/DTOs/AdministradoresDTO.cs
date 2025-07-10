using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class AdministradoresDTO
    {
        public int AdministradorId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string ContraseñaHash { get; set; }
        public string EstadoAdministrador { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public bool? EsSuperAdmin { get; set; }
    }

    public class AdministradorListDTO
    {
        public int AdministradorId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string EstadoAdministrador { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }

    public class AdministradorCreateDTO
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
        public string ContraseñaHash { get; set; }
    }

    public class LoginAdmDTO
    {
        public string CorreoElectronico { get; set; }
        public string ContraseñaHash { get; set; }
    }

    public class AuthAdmResponseDTO
    {
        public string Token { get; set; }
        public string Message { get; set; }
    }

    public class ResetPasswordAdmDTO
    {
        public string CorreoElectronico { get; set; }
        public string NuevaContraseña { get; set; }
    }
}
