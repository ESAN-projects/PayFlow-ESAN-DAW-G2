using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Infrastructure.Repositories
{
    public class AdministradoresRepository : IAdministradoresRepository
    {
        private readonly PayflowContext _context;
        public AdministradoresRepository(PayflowContext context)
        {
            _context = context;
        }

        // Obtener todos los administradores
        public async Task<IEnumerable<Administradores>> GetAllAdministradoresAsync()
        {
            return await _context.Administradores.ToListAsync();
        }

        // Obtener administrador por ID
        public async Task<Administradores?> GetAdministradorByIdAsync(int id)
        {
            return await _context.Administradores.FirstOrDefaultAsync(a => a.AdministradorId == id);
        }

        // Agregar administrador
        public async Task<int> AddAdministradorAsync(Administradores administrador)
        {
            await _context.Administradores.AddAsync(administrador);
            await _context.SaveChangesAsync();
            return administrador.AdministradorId;
        }

        // Actualizar administrador
        public async Task<bool> UpdateAdministradorAsync(Administradores administrador)
        {
            var existingAdministrador = await _context.Administradores.FindAsync(administrador.AdministradorId);
            if (existingAdministrador == null)
            {
                return false;
            }
            existingAdministrador.Nombres = administrador.Nombres;
            existingAdministrador.Apellidos = administrador.Apellidos;
            existingAdministrador.CorreoElectronico = administrador.CorreoElectronico;
            existingAdministrador.ContraseñaHash = administrador.ContraseñaHash;
            existingAdministrador.EstadoAdministrador = administrador.EstadoAdministrador;
            existingAdministrador.FechaRegistro = administrador.FechaRegistro;
            existingAdministrador.EsSuperAdmin = administrador.EsSuperAdmin;
            _context.Administradores.Update(existingAdministrador);
            await _context.SaveChangesAsync();
            return true;
        }

        // Eliminar administrador
        public async Task<bool> DeleteAdministradorAsync(int id)
        {
            var administrador = await _context.Administradores.FindAsync(id);
            if (administrador == null)
            {
                return false;
            }
            _context.Administradores.Remove(administrador);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
