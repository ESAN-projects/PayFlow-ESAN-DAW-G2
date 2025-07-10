using Microsoft.AspNetCore.Http;
using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using PayFlow.DOMAIN.Infrastructure.Repositories;
using System.Security.Claims;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class AdministradorService : IAdministradorService
    {
        private readonly IAdministradoresRepository _administradoresRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly ITransaccionesRepository _transaccionesRepository;
        private readonly ICuentasRepository _cuentasRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdministradorService(IAdministradoresRepository administradoresRepository, JwtTokenGenerator jwtTokenGenerator,ITransaccionesRepository transaccionesRepository, ICuentasRepository cuentasRepository, IHttpContextAccessor httpContextAccessor)
        {
            _administradoresRepository = administradoresRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _transaccionesRepository = transaccionesRepository;
            _cuentasRepository = cuentasRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        //Get All ADM
        public async Task<IEnumerable<AdministradorListDTO>> GetAllAdministradoresAsync(string? filtro, string? busqueda)
        {
            var administradores = await _administradoresRepository.GetAllAdministradoresAsync(filtro, busqueda);
            var administradoresDTO = administradores.Select(c => new AdministradorListDTO
            {
                AdministradorId = c.AdministradorId,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                CorreoElectronico = c.CorreoElectronico,
                EstadoAdministrador = c.EstadoAdministrador,
                FechaRegistro = c.FechaRegistro
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

        // Es administrador?
        public async Task<bool> IsUserAdminAsync()
        {
            // Obtener el idAdministrador desde el JWT
            var isAdministrador = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(isAdministrador))
            {
                throw new Exception("Administrador no autenticado.");
            }

            // Verificar que el claim `Role` sea "Administrador"
            var roleClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (roleClaim != "Administrador")
            {
                throw new UnauthorizedAccessException("El usuario no tiene permisos de administrador.");
            }

            // Convertir el idAdministrador a entero
            int idAdministrador = int.Parse(isAdministrador);

            // Verificar si el administrador existe en la base de datos
            var administrador = await _administradoresRepository.GetAdministradoresByIdAsync(idAdministrador);
            if (administrador == null)
            {
                throw new Exception("Administrador no encontrado.");
            }

            return true;  // El administrador está autenticado y existe en la base de datos
        }

        public async Task<bool> AceptarDepositoAsync(int transaccionId)
        {
            // Verificar si el usuario es un administrador autenticado
            if (!await IsUserAdminAsync())
            {
                throw new UnauthorizedAccessException("No tiene permisos para realizar esta acción.");
            }
            

            // Obtener la transacción pendiente
            var transaccion = await _transaccionesRepository.GetTransaccionById(transaccionId);
            if (transaccion == null || transaccion.Estado != "Pendiente")
            {
                throw new Exception("La transacción no está pendiente o no existe.");
            }

            // Cambiar el estado de la transacción a "Aceptado"
            transaccion.Estado = "Aceptado";
            var resultTransaccion = await _transaccionesRepository.UpdateTransaccion(transaccion);

            if (!resultTransaccion)
            {
                throw new Exception("No se pudo actualizar el estado de la transacción.");
            }

            // Obtener la cuenta asociada
            var cuenta = await _cuentasRepository.GetCuentaByIdAsync(transaccion.CuentaId);
            if (cuenta == null)
            {
                throw new Exception("Cuenta no encontrada.");
            }

            // Actualizar el saldo de la cuenta con el monto de la transacción
            cuenta.Saldo += transaccion.Monto;
            await _cuentasRepository.UpdateCuentaAsync(cuenta); //Modificado al modificar API TRANSACCIONES
            /*var resultCuenta = await _cuentasRepository.UpdateCuentaAsync(cuenta);

            if (!resultCuenta)
            {
                throw new Exception("No se pudo actualizar el saldo de la cuenta.");
            }

            return true; // Indica que el depósito ha sido aceptado y procesado correctamente.
            */
            return true;
        }

    }
}
