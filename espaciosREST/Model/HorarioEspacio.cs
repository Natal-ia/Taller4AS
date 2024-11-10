using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosRest.Model
{
    [Table("horarioEspacio")]
    public class HorarioEspacio
    {
        public int? id { get; set; }
        public bool Disponibilidad { get; set; }
        public TimeSpan? horaInicio { get; set; }
        public TimeSpan? horaFin { get; set; }
        [ForeignKey("Espacio")]
        [Column("espacio_id")] // Asegúrate de que el nombre de la columna coincida con el de la base de datos
        public int EspacioId { get; set; }
        public Espacio? Espacio { get; set; }
    }
}