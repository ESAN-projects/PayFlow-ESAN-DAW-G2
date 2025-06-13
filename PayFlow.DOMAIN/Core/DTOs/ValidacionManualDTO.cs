using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.DOMAIN.Core.DTOs
{
    public class ValidacionManualDTO
    {
        public int TransaccionID { get; set; }
        public int AdministradorID { get; set; }
        public string Resultado { get; set; } = null!;
        public string Comentarios { get; set; } = null!;
    }
}
