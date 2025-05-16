using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Infrastructure.Data;

namespace PayFlow.DOMAIN.Infrastructure.Repositories
{
    internal class AdministradoresRepository
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
            administradores.EstadoAdministrador = "Activo";
            await _context.Administradores.AddAsync(administradores);
            await _context.SaveChangesAsync();
            return administradores;
        }

        //Update Administradores
        public async Task<Administradores> UpdateAdministradoresAsync(Administradores administradores)
        {
            var existingAdministrador = await GetAdministradoresByIdAsync(administradores.AdministradorId);
            if (existingAdministrador != null)
                
            existingAdministrador.Nombres = administradores.Nombres;
            existingAdministrador.Apellidos = administradores.Apellidos;
            existingAdministrador.CorreoElectronico = administradores.CorreoElectronico;
            existingAdministrador.ContraseñaHash = administradores.ContraseñaHash;
            existingAdministrador.FechaRegistro = DateTime.Now;
            await _context.SaveChangesAsync();       
            return existingAdministrador;
        }

        // CRUD operations for Category
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
