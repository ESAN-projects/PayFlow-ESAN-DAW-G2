using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayFlow.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly IConfiguration _configuration;
        public AuthController(IUsuariosRepository usuariosRepository, IConfiguration configuration)
        {
            _usuariosRepository = usuariosRepository;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDto)
        {
            var usuarios = await _usuariosRepository.GetAllUsuariosAsync(null, null, null, null);
            var usuario = usuarios.FirstOrDefault(u => u.CorreoElectronico == loginDto.CorreoElectronico && u.ContraseñaHash == loginDto.Contraseña);
            if (usuario == null)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.CorreoElectronico),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWTSettings:Issuer"],
                audience: _configuration["JWTSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new LoginResponseDTO
            {
                Token = tokenString,
                UsuarioId = usuario.UsuarioId,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                CorreoElectronico = usuario.CorreoElectronico
            });
        }
    }
}
