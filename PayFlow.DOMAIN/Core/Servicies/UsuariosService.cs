using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosRepository _usuariosRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        public UsuariosService(IUsuariosRepository usuariosRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _usuariosRepository = usuariosRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        //Get all usuarios
        public async Task<IEnumerable<UsuariosListDTO>> GetAllUsuariosAsync()
        {
            var usuarios = await _usuariosRepository.GetAllUsuariosAsync();
            var usuariosListDTO = usuarios.Select(usuario => new UsuariosListDTO
            {
                UsuarioId = usuario.UsuarioId,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Dni = usuario.Dni,
                CorreoElectronico = usuario.CorreoElectronico
            });
            return usuariosListDTO;
        }

        //Get by id usuarios
        public async Task<UsuariosListDTO?> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _usuariosRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return null;
            }
            var usuarioDTO = new UsuariosListDTO
            {
                UsuarioId = usuario.UsuarioId,
                Nombres = usuario.Nombres,
                Apellidos = usuario.Apellidos,
                Dni = usuario.Dni,
                CorreoElectronico = usuario.CorreoElectronico,
            };
            return usuarioDTO;
        }

        //Add usuarios
        public async Task<int> AddUsuarioAsync(UsuariosCreateDTO usuarioCreateDTO)
        {
            // Validar si el correo ya está registrado
            var existe = await _usuariosRepository.GetUsuarioByEmailAsync(usuarioCreateDTO.CorreoElectronico);
            if (existe != null)
            {
                // Retornar 0 para indicar que el correo ya existe
                return 0;
            }
            var usuario = new Usuarios
            {
                Nombres = usuarioCreateDTO.Nombres,
                Apellidos = usuarioCreateDTO.Apellidos,
                Dni = usuarioCreateDTO.Dni,
                CorreoElectronico = usuarioCreateDTO.CorreoElectronico,
                ContraseñaHash = BCrypt.Net.BCrypt.HashPassword(usuarioCreateDTO.ContraseñaHash),
                EstadoUsuario = "Activo"
            };
            var usuarioID = await _usuariosRepository.AddUsuarioAsync(usuario);
            return usuarioID;
        }

        //Update usuarios
        public async Task<bool> UpdateUsuarioAsync(UsuariosUpdateDTO usuarioUpdateDTO)
        {
            var usuario = new Usuarios
            {
                UsuarioId = usuarioUpdateDTO.UsuarioId,
                Nombres = usuarioUpdateDTO.Nombres,
                Apellidos = usuarioUpdateDTO.Apellidos,
                Dni = usuarioUpdateDTO.Dni,
                CorreoElectronico = usuarioUpdateDTO.CorreoElectronico,
                ContraseñaHash = usuarioUpdateDTO.ContraseñaHash,
                EstadoUsuario = "Activo"
            };
            var result = await _usuariosRepository.UpdateUsuarioAsync(usuario);
            return result;
        }

        //Delete usuarios
        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await _usuariosRepository.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return false;
            }
            var result = await _usuariosRepository.DeleteUsuarioAsync(id);
            return result;
        }
        //Actualizar Perfil de Usuario
        public async Task<bool> ActualizarPerfilAsync(int usuarioId, PerfilUpdateDTO dto)
        {
            var usuario = await _usuariosRepository.GetUsuarioByIdAsync(usuarioId);
            if (usuario == null)
                return false;
            usuario.Nombres = dto.Nombres;
            usuario.Apellidos = dto.Apellidos;
            usuario.Dni = dto.Dni;
            usuario.CorreoElectronico = dto.CorreoElectronico;

            return await _usuariosRepository.UpdateUsuarioAsync(usuario);
        }


        //Login usuarios
        public async Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginDTO)
        {
            var usuario = await _usuariosRepository.GetUsuarioByEmailAsync(loginDTO.CorreoElectronico);
            if (usuario == null || usuario.EstadoUsuario == "Inactivo")
            {
                return new AuthResponseDTO
                {
                    Message = "Credenciales incorrectas."
                };
            }

            //var result = BCrypt.Net.BCrypt.Verify(loginDTO.ContraseñaHash, usuario.ContraseñaHash);

            bool result;

            // Si la contraseña en BD parece un hash válido de BCrypt (inicia con $2a, $2b, $2y o similar)
            if (!string.IsNullOrWhiteSpace(usuario.ContraseñaHash) && usuario.ContraseñaHash.StartsWith("$2"))
            {
                result = BCrypt.Net.BCrypt.Verify(loginDTO.Contraseña, usuario.ContraseñaHash);
            }
            else
            {
                // Comparación directa (solo para pruebas)
                result = usuario.ContraseñaHash == loginDTO.Contraseña;
            }

            if (!result)  // Si result es false, las credenciales son incorrectas.
            {
                return new AuthResponseDTO
                {
                    Message = "Credenciales incorrectas."
                };
            }

            var token = _jwtTokenGenerator.GenerateJwtToken(usuario.CorreoElectronico, usuario.UsuarioId, "Usuario");

            return new AuthResponseDTO
            {
                Token = token,
                Message = "Autenticación exitosa."
            };
        }

        //Resetear contraseña con enlace de contraseña
        public async Task<string> ResetPasswordAsync(ResetPasswordDTO resetPasswordDTO)
        {
            // Buscar solo usuarios activos
            var usuario = await _usuariosRepository.GetUsuarioByCorreoAsync(resetPasswordDTO.CorreoElectronico);
            if (usuario == null)
            {
                return "Usuario no encontrado o inactivo.";
            }
            // Actualizar el usuario en la base de datos
            var result = await _usuariosRepository.ResetPassword(resetPasswordDTO.CorreoElectronico, resetPasswordDTO.NuevaContraseña);
            if (!result)
            {
                return "Error al restablecer la contraseña.";
            }
            return "Contraseña restablecida con éxito.";
        }
    }
}
