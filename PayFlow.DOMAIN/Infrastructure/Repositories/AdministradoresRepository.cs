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
        /*public async Task<Administradores> UpdateAdministradoresAsync(Administradores administradores)
        {
            var existingAdministrador = await GetAdministradoresByIdAsync(administradores.AdministradorId);
            if (existingAdministrador != null)
            {

                existingAdministrador.Nombres = administradores.Nombres;
                existingAdministrador.Apellidos = administradores.Apellidos;
                existingAdministrador.CorreoElectronico = administradores.CorreoElectronico;
                existingAdministrador.ContraseñaHash = administradores.ContraseñaHash;
                existingAdministrador.FechaRegistro = DateTime.Now;
                existingAdministrador.EsSuperAdmin = administradores.EsSuperAdmin;
                await _context.SaveChangesAsync();
            }
            return existingAdministrador;
        }*/
        public async Task<Administradores> UpdateAdministradoresAsync(Administradores administradores)
        {
            _context.Administradores.Update(administradores);
            await _context.SaveChangesAsync();
            return administradores;
        }
        //Delete Administradores by id (borrado logico)
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

        

        // Delete Administradores (borrado físico)
        public async Task<bool> Delete2AdministradoresAsync(int id)
        {
            var administradores = await GetAdministradoresByIdAsync(id);
            if (administradores == null)
            {
                return false;
            }

            _context.Administradores.Remove(administradores); 
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
