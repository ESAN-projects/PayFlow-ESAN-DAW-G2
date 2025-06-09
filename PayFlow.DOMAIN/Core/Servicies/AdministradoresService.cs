using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class AdministradoresService : IAdministradoresService
    {
        private readonly IAdministradoresRepository _administradoresRepository;
        public AdministradoresService(IAdministradoresRepository administradoresRepository)
        {
            _administradoresRepository = administradoresRepository;
        }

        // Obtener todos los administradores
        public async Task<IEnumerable<AdministradoresListDTO>> GetAllAdministradoresAsync()
        {
            var administradores = await _administradoresRepository.GetAllAdministradoresAsync();
            return administradores.Select(admin => new AdministradoresListDTO
            {
                AdministradorId = admin.AdministradorId,
                Nombres = admin.Nombres,
                Apellidos = admin.Apellidos,
                CorreoElectronico = admin.CorreoElectronico,
                EstadoAdministrador = admin.EstadoAdministrador,
                EsSuperAdmin = admin.EsSuperAdmin
            });
        }

        // Obtener administrador por ID
        public async Task<AdministradoresListDTO?> GetAdministradorByIdAsync(int id)
        {
            var admin = await _administradoresRepository.GetAdministradorByIdAsync(id);
            if (admin == null) return null;
            return new AdministradoresListDTO
            {
                AdministradorId = admin.AdministradorId,
                Nombres = admin.Nombres,
                Apellidos = admin.Apellidos,
                CorreoElectronico = admin.CorreoElectronico,
                EstadoAdministrador = admin.EstadoAdministrador,
                EsSuperAdmin = admin.EsSuperAdmin
            };
        }

        // Agregar administrador
        public async Task<int> AddAdministradorAsync(AdministradoresCreateDTO adminCreateDTO)
        {
            var admin = new Administradores
            {
                Nombres = adminCreateDTO.Nombres,
                Apellidos = adminCreateDTO.Apellidos,
                CorreoElectronico = adminCreateDTO.CorreoElectronico,
                ContraseñaHash = adminCreateDTO.ContraseñaHash,
                EstadoAdministrador = adminCreateDTO.EstadoAdministrador,
                FechaRegistro = adminCreateDTO.FechaRegistro,
                EsSuperAdmin = adminCreateDTO.EsSuperAdmin
            };
            return await _administradoresRepository.AddAdministradorAsync(admin);
        }

        // Actualizar administrador
        public async Task<bool> UpdateAdministradorAsync(AdministradoresUpdateDTO adminUpdateDTO)
        {
            var admin = new Administradores
            {
                AdministradorId = adminUpdateDTO.AdministradorId,
                Nombres = adminUpdateDTO.Nombres,
                Apellidos = adminUpdateDTO.Apellidos,
                CorreoElectronico = adminUpdateDTO.CorreoElectronico,
                ContraseñaHash = adminUpdateDTO.ContraseñaHash,
                EstadoAdministrador = adminUpdateDTO.EstadoAdministrador,
                FechaRegistro = adminUpdateDTO.FechaRegistro,
                EsSuperAdmin = adminUpdateDTO.EsSuperAdmin
            };
            return await _administradoresRepository.UpdateAdministradorAsync(admin);
        }

        // Eliminar administrador
        public async Task<bool> DeleteAdministradorAsync(int id)
        {
            var admin = await _administradoresRepository.GetAdministradorByIdAsync(id);
            if (admin == null) return false;
            return await _administradoresRepository.DeleteAdministradorAsync(id);
        }
    }
}
