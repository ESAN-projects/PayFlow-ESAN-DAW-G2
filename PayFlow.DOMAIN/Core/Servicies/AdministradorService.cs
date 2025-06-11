using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradoresRepository _administradoresRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AdministradorService(IAdministradoresRepository administradoresRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _administradoresRepository = administradoresRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
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

        public async Task<AuthAdmResponseDTO> LoginAsync(LoginAdmDTO loginAdmDTO)
        {
            var administrador = await _administradoresRepository.GetAdministradorByEmailAsync(loginAdmDTO.CorreoElectronico);
            if (administrador == null || administrador.EstadoAdministrador == "Inactivo")
            {
                return new AuthAdmResponseDTO
                {
                    Message = "Credenciales incorrectas."
                };
            }

            var result = BCrypt.Net.BCrypt.Verify(loginAdmDTO.ContraseñaHash, administrador.ContraseñaHash);
            if (!result)
            {
                return new AuthAdmResponseDTO
                {
                    Message = "Credenciales incorrectas."
                };
            }

            // Generar JWT Token para Administrador
            var token = _jwtTokenGenerator.GenerateJwtToken(administrador.CorreoElectronico, administrador.AdministradorId, "Administrador");

            return new AuthAdmResponseDTO
            {
                Token = token,
                Message = "Autenticación exitosa."
            };
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordAdmDTO resetPasswordAdmDTO)
        {
            var administrador = await _administradoresRepository.GetAdministradorByEmailAsync(resetPasswordAdmDTO.CorreoElectronico);
            if (administrador == null || administrador.EstadoAdministrador == "Inactivo")
            {
                return "Administrador no encontrado o inactivo.";
            }
            // Actualizar el administrador en la base de datos
            var result = await _administradoresRepository.ResetPassword(resetPasswordAdmDTO.CorreoElectronico, resetPasswordAdmDTO.NuevaContraseña);
            if (!result)
            {
                return "Error al restablecer la contraseña.";
            }
            return "Contraseña restablecida con éxito.";
        }

    }
}
