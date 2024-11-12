using Grpc.Core;
using servicioDatos;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using servicioDatos.Model;

namespace servicioDatos.Services
{
    public class EspacioServiceImpl : EspacioService.EspacioServiceBase
    {
        private readonly ILogger<EspacioServiceImpl> _logger;
        private readonly EspacioDbContext _dbContext;

        public EspacioServiceImpl(ILogger<EspacioServiceImpl> logger, EspacioDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // Implementación de GetEspacios
        public override async Task<EspaciosResponse> GetEspacios(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        {
            var espacios = await _dbContext.Espacios.ToListAsync(); 
            var response = new EspaciosResponse();

            response.Espacios.AddRange(espacios.Select(e => new Espacio
            {
                Id = e.id,
                Nombre = e.nombre,
                Descripcion = e.descripcion,
                HoraApertura = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(e.horaApertura),
                HoraCierre = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(e.horaCierre)
            }));
            
            return response;
        }

            public override async Task<EspacioResponse> GetEspacioById(EspacioIdRequest request, ServerCallContext context)
            {
                var espacio = await _dbContext.Espacios
                    .Include(e => e.Horarios) 
                    .FirstOrDefaultAsync(e => e.id == request.Id);

                if (espacio == null)
                {
                    return new EspacioResponse
                    {
                        Success = false,
                        Message = "Espacio no encontrado"
                    };
                }

                var espacioGrpc = new Espacio
                {
                    Id = espacio.id,
                    Nombre = espacio.nombre,
                    Descripcion = espacio.descripcion,
                    HoraApertura = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(espacio.horaApertura),
                    HoraCierre = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(espacio.horaCierre),
                    Horarios = { espacio.Horarios.Select(h => new HorarioEspacio
                    {
                        Id = h.id,
                        Disponibilidad = h.disponibilidad,
                        HoraInicio = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(h.horaInicio),
                        HoraFin = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(h.horaFin),
                        EspacioId = h.espacio_id
                    })}
                };
                return new EspacioResponse
                {
                    Espacio = espacioGrpc,
                    Success = true,
                    Message = "Espacio encontrado"
                };
            }
            public override async Task<EspaciosResponse> ListWithHorarios(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
            {
                // Cargar los espacios con sus horarios relacionados
                var espacios = await _dbContext.Espacios.Include(e => e.Horarios).ToListAsync();
                var response = new EspaciosResponse();
                
                foreach (var e in espacios)
                {
                    var espacio = new Espacio
                    {
                        Id = e.id,
                        Nombre = e.nombre,
                        Descripcion = e.descripcion,
                        HoraApertura = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(e.horaApertura),
                        HoraCierre = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(e.horaCierre)
                    };

                    // Asignar explícitamente la lista de horarios
                    espacio.Horarios.AddRange(e.Horarios.Select(h => new HorarioEspacio
                    {
                        Id = h.id,
                        Disponibilidad = h.disponibilidad,
                        HoraInicio = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(h.horaInicio),
                        HoraFin = Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan(h.horaFin),
                        EspacioId = e.id
                    }));

                    response.Espacios.Add(espacio);
                }

                return response;
            }

            public override async Task<DisponibilidadResponse> UpdateDisponibilidad(DisponibilidadRequest request, ServerCallContext context)
            {
                var espacio = await _dbContext.Espacios
                    .Include(e => e.Horarios)
                    .FirstOrDefaultAsync(e => e.id == request.IdEspacio);

                if (espacio == null)
                {
                    _logger.LogWarning($"Espacio con id {request.IdEspacio} no encontrado.");
                    return new DisponibilidadResponse
                    {
                        Success = false,
                        Message = $"Espacio con id {request.IdEspacio} no encontrado."
                    };
                }

                var horarioEspacio = espacio.Horarios?.FirstOrDefault(h => h.id == request.IdHorario);

                if (horarioEspacio == null)
                {
                    _logger.LogWarning($"Horario con id {request.IdHorario} no encontrado en el espacio con id {request.IdEspacio}.");
                    return new DisponibilidadResponse
                    {
                        Success = false,
                        Message = $"Horario con id {request.IdHorario} no encontrado en el espacio con id {request.IdEspacio}."
                    };
                }

                // Actualizar disponibilidad
                horarioEspacio.disponibilidad = request.Disponibilidad;
                try
                {
                    await _dbContext.SaveChangesAsync();
                    return new DisponibilidadResponse
                    {
                        Success = true,
                        Message = "Disponibilidad actualizada correctamente."
                    };
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar la disponibilidad en la base de datos.");
                    return new DisponibilidadResponse
                    {
                        Success = false,
                        Message = "Hubo un error al actualizar la disponibilidad."
                    };
                }
            }
    


    }
}