using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Data;

namespace PayFlow.DOMAIN.Infrastructure.Repositories
{
    public class AdministradoresRepository : IAdministradoresRepository
    {
        private readonly PayflowContext _context;
        public AdministradoresRepository(PayflowContext context)
        {
            _context = context;
        }

        //Get all Administradores
        public async Task<List<Administradores>> GetAllAdministradoresAsync()
        {
            return await _context.Administradores.Where(c => c.EstadoAdministrador == "Activo").ToListAsync();
        }

        //Get Administradores by ID
        public async Task<Administradores> GetAdministradoresByIdAsync(int id)
        {
            return await _context.Administradores.Where(c => c.AdministradorId == id && c.EstadoAdministrador == "Activo").FirstOrDefaultAsync();
        }

        //Add Administradores
        public async Task<Administradores> AddAdministradoresAsync(Administradores administradores)
        {
            if (administradores == null)
                throw new ArgumentNullException(nameof(administradores));

            administradores.EstadoAdministrador = "Activo";
            await _context.Administradores.AddAsync(administradores);
            await _context.SaveChangesAsync();
            return administradores;
        }

        //Update Administradores
        public async Task<bool> UpdateAdministradoresAsync(Administradores administrador)
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
        //Delete Administradores
        public async Task<bool> DeleteAdministradoresAsync(int id)
        {
            var administradores = await GetAdministradoresByIdAsync(id);
            if (administradores == null)
            {
                return false;
            }
            administradores.EstadoAdministrador = "Inactivo";
            _context.Administradores.Update(administradores);
            await _context.SaveChangesAsync();
            return true;
        }

        // Delete Administradores by id (borrado logico)
        public async Task<bool> RemoveAdministradoresAsync(int id)
        {
            var administradores = await GetAdministradoresByIdAsync(id);
            if (administradores == null)
            {
                return false;
            }

            administradores.EstadoAdministrador = "Inactivo";
            _context.Administradores.Update(administradores);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
