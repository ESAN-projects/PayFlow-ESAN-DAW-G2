namespace PayFlow.DOMAIN.Core.DTOs
{
    public class UsuariosDTO
    {
        public int UsuarioId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Dni { get; set; }
        public string CorreoElectronico { get; set; }
        public string ContraseñaHash { get; set; }
        public string EstadoUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    public class UsuariosListDTO
    {
        public int UsuarioId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Dni { get; set; }
        public string CorreoElectronico { get; set; }
    }

    public class UsuariosCreateDTO
    {
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Dni { get; set; }
        public string CorreoElectronico { get; set; }
        public string ContraseñaHash { get; set; }
        public string EstadoUsuario { get; set; }
    }

    public class UsuariosUpdateDTO 
    {
        public int UsuarioId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Dni { get; set; }
        public string CorreoElectronico { get; set; }
        public string ContraseñaHash { get; set; }
        public string EstadoUsuario { get; set; }
    }
}
