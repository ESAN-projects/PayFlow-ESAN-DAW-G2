using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;

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
        public async Task<IEnumerable<Usuarios>> GetAllUsuariosAsync()
        {
            return await _context.Usuarios.Where(x => x.EstadoUsuario == "Activo").ToListAsync();
        }

        //Get by id usuarios
        public async Task<Usuarios?> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(x => x.UsuarioId == id && x.EstadoUsuario == "Activo");
        }

        //Add usuarios
        public async Task<int> AddUsuarioAsync(Usuarios usuario)
        {
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
    }
}
