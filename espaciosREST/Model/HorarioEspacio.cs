using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosRest.Model
{
    [Table("horarioEspacio")]
    public class HorarioEspacio
    {
        public int? id { get; set; }
        public bool disponibilidad { get; set; }
        public TimeSpan? horaInicio { get; set; }
        public TimeSpan? horaFin { get; set; }
        [ForeignKey("Espacio")]
        public int espacio_id { get; set; }
    }
}