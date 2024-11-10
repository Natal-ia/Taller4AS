namespace RazorUI.Models
{
    public class HorarioEspacioViewModel
    {
        public int Id { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFin { get; set; }
        public bool Disponibilidad { get; set; }  
    }
}
