using Microsoft.EntityFrameworkCore;
using EventosRest.Model;
using Microsoft.AspNetCore.Mvc;

namespace EventosRest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EspaciosController : ControllerBase
    {
        private readonly EspacioDbContext _espacioDbContext;
        private readonly ILogger<EspaciosController> _logger;

        public EspaciosController(ILogger<EspaciosController> logger, EspacioDbContext espacioDbContext)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _espacioDbContext = espacioDbContext ?? throw new ArgumentNullException(nameof(espacioDbContext));
        }

        [HttpGet(Name = "GetCursos")]
        public async Task<List<Espacio>> Get()
        {
            var espacios = await _espacioDbContext.Espacios.ToListAsync();
            return espacios;
        }


        [HttpGet("{id}", Name = "GetEspacioById")]
        public async Task<ActionResult<Espacio>> GetById(int id)
        {
            var espacio = await _espacioDbContext.Espacios.FindAsync(id);
            if (espacio == null)
            {
                return NotFound();
            }
            return espacio;
        }
    }
}