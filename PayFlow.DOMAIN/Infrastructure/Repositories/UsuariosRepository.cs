using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PayFlow.DOMAIN.Infrastructure.Repositories
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly PayflowContext _context;
        public UsuariosRepository(PayflowContext context)
        {
            _context = context;
        }

        //Get all usuarios
        public async Task<IEnumerable<Usuarios>> GetAllUsuariosAsync(string? filtro, string? busqueda, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = _context.Usuarios.AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
                query = query.Where(x => x.EstadoUsuario == filtro);

            if (!string.IsNullOrEmpty(busqueda))
                query = query.Where(x => x.Nombres.Contains(busqueda) || x.Apellidos.Contains(busqueda));

            if (fechaInicio.HasValue)
                query = query.Where(x => x.FechaRegistro >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(x => x.FechaRegistro <= fechaFin.Value);

            return await query.ToListAsync();
        }

        //Get by id usuarios
        public async Task<Usuarios?> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id && x.EstadoUsuario == "Activo");
        }

        //Get by id usuarios
        public async Task<Usuarios?> GetUsuarioByCorreoAsync(string Email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.CorreoElectronico == Email && x.EstadoUsuario == "Activo");
        }

        //Add usuarios
        public async Task<int> AddUsuarioAsync(Usuarios usuario)
        {
            // Validar si el correo ya está registrado (activo o inactivo)
            var existeCorreo = await _context.Usuarios.AnyAsync(x => x.CorreoElectronico == usuario.CorreoElectronico);
            if (existeCorreo)
            {
                // Retornar 0 para indicar que el correo ya existe
                return 0;
            }
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario.UsuarioId;
        }

        //Update usuarios
        public async Task<bool> UpdateUsuarioAsync(Usuarios usuario)
        {
            var existingUsuario = await _context.Usuarios.FindAsync(usuario.UsuarioId);
            if (existingUsuario == null)
            {
                return false;
            }
            existingUsuario.Nombres = usuario.Nombres;
            existingUsuario.Apellidos = usuario.Apellidos;
            existingUsuario.Dni = usuario.Dni;
            existingUsuario.CorreoElectronico = usuario.CorreoElectronico;
            existingUsuario.ContraseñaHash = usuario.ContraseñaHash;
            existingUsuario.EstadoUsuario = usuario.EstadoUsuario;
            _context.Usuarios.Update(existingUsuario);
            await _context.SaveChangesAsync();
            return true;
        }

        //Delete usuarios
        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return false;
            }
            usuario.EstadoUsuario = "Inactivo";
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        //get usuario by email
        public async Task<Usuarios?> GetUsuarioByEmailAsync(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.CorreoElectronico == email && x.EstadoUsuario == "Activo");
        }

        //reset password
        public async Task<bool> ResetPassword(string correo, string newPassword)
        {
            // Solo permitir reset si el usuario está activo
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.CorreoElectronico == correo && x.EstadoUsuario == "Activo");
            if (usuario == null)
            {
                return false;
            }

            // Generar un nuevo hash de contraseña
            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            usuario.ContraseñaHash = newPasswordHash;
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        //Obtener usuario por JWT
        public async Task<Usuarios?> GetUsuarioByJwtTokenAsync(string jwtToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);
                var usuarioIdClaim = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (usuarioIdClaim != null && int.TryParse(usuarioIdClaim.Value, out int usuarioId))
                {
                    return await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.EstadoUsuario == "Activo");
                }
                return null;
            }
            catch
            {
                return null; // Si hay un error al procesar el token, retornar null
            }
        }
    }
}
