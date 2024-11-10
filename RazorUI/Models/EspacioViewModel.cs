namespace RazorUI.Models
{
    public class EspacioViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty; // Valor predeterminado
        public TimeSpan? HoraApertura { get; set; }
        public TimeSpan? HoraCierre { get; set; }
        public List<HorarioEspacioViewModel> Horarios { get; set; } = new List<HorarioEspacioViewModel>();
    }
}
