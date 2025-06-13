<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Repositories;
=======
﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
>>>>>>> bb7536d3585a20b6fc434edcd3b09fcf90c48232

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
            var usuario = new Usuarios
            {
                Nombres = usuarioCreateDTO.Nombres,
                Apellidos = usuarioCreateDTO.Apellidos,
                Dni = usuarioCreateDTO.Dni,
                CorreoElectronico = usuarioCreateDTO.CorreoElectronico,
                ContraseñaHash = usuarioCreateDTO.ContraseñaHash,
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
<<<<<<< HEAD
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

=======

        //Login usuarios
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var usuario = await _usuariosRepository.GetUsuarioByEmailAsync(loginDTO.CorreoElectronico);
            if (usuario == null || usuario.EstadoUsuario == "Inactivo")
            {
                return new AuthResponseDTO
                {
                    Message = "Credenciales incorrectas."
                };
            }

            var result = BCrypt.Net.BCrypt.Verify(loginDTO.ContraseñaHash, usuario.ContraseñaHash);

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
            var usuario = await _usuariosRepository.GetUsuarioByEmailAsync(resetPasswordDTO.CorreoElectronico);
            if (usuario == null || usuario.EstadoUsuario == "Inactivo")
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
>>>>>>> bb7536d3585a20b6fc434edcd3b09fcf90c48232
    }
}
