namespace PayFlow.DOMAIN.Core.DTOs
{
    public class LoginRequestDTO
    {
        public string CorreoElectronico { get; set; }
        public string Contraseña { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public int UsuarioId { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string CorreoElectronico { get; set; }
    }
}
