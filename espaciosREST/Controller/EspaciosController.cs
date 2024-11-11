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

        [HttpGet(Name = "GetEspacios")]
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

        [HttpGet("ListWithHorarios")]
        public async Task<List<Espacio>> GetEspaciosConHorarios()
        {
            var espacios = await _espacioDbContext.Espacios
                .Include(e => e.Horarios)  // Incluye los horarios
                .ToListAsync();

            if (espacios == null || !espacios.Any())
            {
                _logger.LogWarning("No se encontraron espacios con horarios.");
            }
            else
            {
                _logger.LogInformation($"{espacios.Count} espacios cargados con horarios.");
            }
            return espacios!;
        }

        [HttpPut("UpdateDisponibilidad/{espacioId}/{horarioId}")]
        public async Task<IActionResult> UpdateDisponibilidadTrue(int espacioId, int horarioId, [FromBody] bool nuevaDisponibilidad)
        {
            var espacio = await _espacioDbContext.Espacios
                .Include(e => e.Horarios)
                .FirstOrDefaultAsync(e => e.id == espacioId);

            if (espacio == null)
            {
                return NotFound($"Espacio con id {espacioId} no encontrado.");
            }

            var horarioEspacio = espacio.Horarios?.FirstOrDefault(h => h.id == horarioId);

            if (horarioEspacio == null)
            {
                return NotFound($"Horario con id {horarioId} no encontrado en el espacio con id {espacioId}.");
            }
            horarioEspacio.disponibilidad = nuevaDisponibilidad;

            try
            {
                await _espacioDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar los cambios en la base de datos.");
                return StatusCode(500, "Hubo un error al actualizar la disponibilidad.");
            }
            return NoContent(); 
        }
    }

}