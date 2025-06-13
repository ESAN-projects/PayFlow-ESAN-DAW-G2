using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using PayFlow.DOMAIN.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Servicies
{
    public class ValidacionManualService : IValidacionManualService
    {
        private readonly IHistorialValidacionesRepository _repository;

        public ValidacionManualService(IHistorialValidacionesRepository repository)
        {
            _repository = repository;
        }

        // Validar manualmente una transacción
        public async Task ValidarManualAsync(ValidacionManualDTO dto)
        {
            if (dto.Resultado == "Rechazado" && string.IsNullOrEmpty(dto.Comentarios))
                throw new ArgumentException("El comentario es obligatorio al rechazar.");

            var validacion = new HistorialValidaciones
            {
                TransaccionId = dto.TransaccionID,
                AdministradorId = dto.AdministradorID,
                FechaHora = DateTime.UtcNow,
                TipoValidacion = "Manual",
                Resultado = dto.Resultado,
                Comentarios = dto.Comentarios
            };

            

            await _repository.RegistrarValidacionManualAsync(validacion);
        }

        //Obtener historial de validaciones
        public async Task<IEnumerable<HistorialValidaciones>> ObtenerHistorialAsync(FiltroValidacionDTO filtro)
        {
            return await _repository.ObtenerHistorialAsync(filtro);
        }
    }

}
