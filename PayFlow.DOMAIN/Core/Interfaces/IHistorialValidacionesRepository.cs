using PayFlow.DOMAIN.Core.DTOs;
using PayFlow.DOMAIN.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.Interfaces
{
    public interface IHistorialValidacionesRepository
    {

        Task<IEnumerable<HistorialValidaciones>> ObtenerHistorialAsync(FiltroValidacionDTO filtro);
        Task RegistrarValidacionManualAsync(HistorialValidaciones validacion);
    }
}
