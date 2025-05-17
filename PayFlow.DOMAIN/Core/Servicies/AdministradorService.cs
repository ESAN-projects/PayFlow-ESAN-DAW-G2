using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradoresRepository _administradoresRepository;

        public AdministradorService(IAdministradoresRepository administradoresRepository)
        {
            _administradoresRepository = administradoresRepository;
        }
        //Get All ADM
        public async Task<IEnumerable<AdministradorListDTO>> GetAllAdministradoresAsync()
        {
            var administradores = await _administradoresRepository.GetAllAdministradoresAsync();
            var administradoresDTO = administradores.Select(c => new AdministradorListDTO
            {
                AdministradorId = c.AdministradorId,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,

            });
            return administradoresDTO;
        }
        //Get ADM by ID
        public async Task<AdministradorListDTO> GetAdministradoresByIdAsync(int id)
        {
            var administradores = await _administradoresRepository.GetAdministradoresByIdAsync(id);
            if (administradores == null)
            {
                return null;
            }
            var administradorDTO = new AdministradorListDTO
            {
                AdministradorId = administradores.AdministradorId,
                Nombres = administradores.Nombres,
                Apellidos = administradores.Apellidos,
            };
            return administradorDTO;
        }
        //Add administradores
        public async Task<Administradores> AddAdministradoresAsync(AdministradorCreateDTO data)
        {
            var administradores = new Administradores
            {
                Nombres = data.Nombres,
                Apellidos = data.Apellidos,
                CorreoElectronico = data.CorreoElectronico,
                ContraseñaHash = data.ContraseñaHash

            };
            return await _administradoresRepository.AddAdministradoresAsync(administradores);
        }


        //Update administradores

        public async Task<bool> UpdateAdministradoresAsync(AdministradorListDTO administradorListDTO)
        {       

            var administrador = new Administradores
            {
                AdministradorId = administradorListDTO.AdministradorId,
                Nombres = administradorListDTO.Nombres,
                Apellidos = administradorListDTO.Apellidos,
                ContraseñaHash = administradorListDTO.ContraseñaHash,
                CorreoElectronico = administradorListDTO.CorreoElectronico,
                EstadoAdministrador = administradorListDTO.EstadoAdministrador,
                FechaRegistro = DateTime.Now,

            };

            return await _administradoresRepository.UpdateAdministradoresAsync(administrador);

        }

        //Delete ADM borrado logico

        public async Task<bool> DeleteAdministradoresAsync(int id)
        {
            return await _administradoresRepository.DeleteAdministradoresAsync(id);
        }
    }
}
