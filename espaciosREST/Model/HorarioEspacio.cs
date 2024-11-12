using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosRest.Model
{
    public class HorarioEspacio
    {
        public int id { get; set; }
        public bool disponibilidad { get; set; }
        public TimeSpan horaInicio { get; set; }
        public TimeSpan horaFin { get; set; }
        public int espacio_id { get; set; }
    }
}