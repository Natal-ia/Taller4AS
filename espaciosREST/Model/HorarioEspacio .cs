using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosRest.Model
{
    [Table("horarioEspacio")]
    public class HorarioEspacio
    {
        public int? id { get; set; }
        public string? disponibilidad { get; set; }
        public TimeSpan? horaInicio { get; set; }
        public TimeSpan? horaFin { get; set; }    
        [ForeignKey("Espacio")]
        public int EspacioId { get; set; }
        public Espacio? Espacio { get; set; }    
    }
}