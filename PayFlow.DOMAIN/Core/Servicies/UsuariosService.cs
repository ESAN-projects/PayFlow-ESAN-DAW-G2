using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Repositories;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class UsuariosService : IUsuariosService
    {
        private readonly IUsuariosRepository _usuariosRepository;
        public UsuariosService(IUsuariosRepository usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
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

    }
}
