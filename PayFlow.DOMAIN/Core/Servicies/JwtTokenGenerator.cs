using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Método genérico para generar un JWT Token, tanto para usuarios como administradores
        public string GenerateJwtToken(string email, int id, string role)
        {
            // Configuración de los Claims
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role), // Role dinámico, puede ser "Usuario" o "Administrador"
        };

            var jwtSettings = _configuration.GetSection("JWTSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Expiración del token
            var expirationMinutes = Convert.ToInt32(jwtSettings["ExpirationMinutes"]);
            var expirationTime = DateTime.UtcNow.AddMinutes(expirationMinutes);

            // Crear el token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expirationTime,
                signingCredentials: credentials
            );

            // Generar y devolver el token como cadena
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
