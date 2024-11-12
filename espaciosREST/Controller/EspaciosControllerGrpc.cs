using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using servicioDatos;
using Grpc.Core;

namespace EventosRest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EspaciosController : ControllerBase // Cambié a ControllerBase
    {
        private readonly EspacioService.EspacioServiceClient _grpcClient;
        private readonly ILogger<EspaciosController> _logger;
        public EspaciosController(ILogger<EspaciosController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                MaxConnectionsPerServer = 10
            };

            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri("http://localhost:5156")
            };

            var channel = GrpcChannel.ForAddress("http://localhost:5156", new GrpcChannelOptions
            {
                HttpClient = httpClient, 
                Credentials = ChannelCredentials.Insecure // Desactivar SSL
            });

            _grpcClient = new EspacioService.EspacioServiceClient(channel);
        }

        [HttpGet(Name = "GetEspacios")]
        public async Task<List<EventosRest.Model.Espacio>> GetEspacios()
        {
            var response = await _grpcClient.GetEspaciosAsync(new Empty());
            return response.Espacios.Select(e => new EventosRest.Model.Espacio { 
                id = e.Id, 
                nombre = e.Nombre, 
                descripcion = e.Descripcion, 
                horaApertura = e.HoraApertura.ToTimeSpan(),
                horaCierre = e.HoraCierre.ToTimeSpan(),
            }).ToList();
        }

        [HttpGet("{id}", Name = "GetEspacioById")]
        public async Task<ActionResult<EventosRest.Model.Espacio>> GetEspacioById(int id)
        {
            var response = await _grpcClient.GetEspacioByIdAsync(new EspacioIdRequest { Id = id });

            if (response == null || response.Espacio == null)
            {
                return NotFound();
            }
            var espacio = new EventosRest.Model.Espacio
            {
                id = response.Espacio.Id,
                nombre = response.Espacio.Nombre,
                descripcion = response.Espacio.Descripcion,
                horaApertura = response.Espacio.HoraApertura.ToTimeSpan(),
                horaCierre = response.Espacio.HoraCierre.ToTimeSpan()
            };

            return espacio; 
        }

        [HttpGet("ListWithHorarios")]
        public async Task<List<EventosRest.Model.Espacio>> ListWithHorarios()
        {
            var response = await _grpcClient.ListWithHorariosAsync (new Empty());
            
            // Depuración para ver si los horarios se están recibiendo correctamente
            _logger.LogWarning($"Número de espacios recibidos: {response.Espacios.Count}");
            foreach (var espacio in response.Espacios)
            {
                _logger.LogWarning($"Espacio: {espacio.Nombre}, Número de horarios: {espacio.Horarios.Count}");
            }

            return response.Espacios.Select(e => new EventosRest.Model.Espacio
            {
                id = e.Id,
                nombre = e.Nombre,
                descripcion = e.Descripcion,
                horaApertura = e.HoraApertura.ToTimeSpan(),
                horaCierre = e.HoraCierre.ToTimeSpan(),
                Horarios = e.Horarios.Select(h => new EventosRest.Model.HorarioEspacio
                {
                    id = h.Id,
                    disponibilidad = h.Disponibilidad,
                    horaInicio = h.HoraInicio.ToTimeSpan(),
                    horaFin = h.HoraFin.ToTimeSpan(),
                    espacio_id = h.EspacioId
                }).ToList()
            }).ToList();
        }

        [HttpPut("UpdateDisponibilidad/{espacioId}/{horarioId}")]
        public async Task<IActionResult> UpdateDisponibilidad(int espacioId, int horarioId, [FromBody] bool nuevaDisponibilidad)
        {
            var request = new DisponibilidadRequest
            {
                IdEspacio = espacioId,
                IdHorario = horarioId,
                Disponibilidad = nuevaDisponibilidad
            };

            try
            {
                var response = await _grpcClient.UpdateDisponibilidadAsync(request);

                if (!response.Success)
                {
                    _logger.LogWarning($"Error al actualizar la disponibilidad: {response.Message}");
                    return BadRequest(response.Message);
                }

                return NoContent(); // Actualización exitosa
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "Error al llamar al servicio gRPC.");
                return StatusCode(500, "Error al comunicarse con el servidor gRPC.");
            }
        }

    }
}
