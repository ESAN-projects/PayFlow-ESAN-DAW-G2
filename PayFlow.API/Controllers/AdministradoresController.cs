using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayFlow.DOMAIN.Infrastructure.Data;
using PayFlow.DOMAIN.Core.Interfaces;

namespace PayFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        private readonly IAdministradoresRepository _administradoresRepository;

        public AdministradoresController(IAdministradoresRepository administradoresRepository)
        {
            _administradoresRepository = administradoresRepository;
        }

        // GET all Administradores
        [HttpGet]
        public async Task<IActionResult> GetAllAdministradores()
        {
            var administradores = await _administradoresRepository.GetAllAdministradoresAsync();    
            return Ok(administradores);
        }
    }
}
