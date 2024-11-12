using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EventosRest.Model
{
    public class Espacio
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? descripcion { get; set; }
        public TimeSpan horaApertura { get; set; }
        public TimeSpan horaCierre { get; set; }
        //[JsonIgnore] // Ignore this property to avoid cycle
        public List<HorarioEspacio> Horarios { get; set; } = new List<HorarioEspacio>();
    }
}