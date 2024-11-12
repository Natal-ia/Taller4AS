using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;
using RazorUI.Services;

namespace RazorUI.Pages
{
    public class EspaciosModel : PageModel
    {
        private readonly EspaciosService _espaciosService;

        public List<EspacioViewModel> Espacios { get; set; }
        public List<HorarioEspacioViewModel> Carrito { get; set; } = new();

        public EspaciosModel(EspaciosService espaciosService)
        {
            _espaciosService = espaciosService;
            Espacios = new List<EspacioViewModel>();
        }

        public async Task OnGetAsync()
        {
            Espacios = await _espaciosService.GetEspaciosWithHorariosAsync();
        }
        public async Task<PageResult> OnPostUpdateDisponibilidadAsync(int espacioId, int horarioId, bool nuevaDisponibilidad)
        {
            //
            var updateSuccess = await _espaciosService.UpdateDisponibilidadAsync(espacioId, horarioId, nuevaDisponibilidad);

            if (updateSuccess)
            {
                // Update the in-memory model to reflect the change in availability
                var espacio = Espacios.FirstOrDefault(e => e.Id == espacioId);
                var horario = espacio?.Horarios.FirstOrDefault(h => h.Id == horarioId);
                if (horario != null)
                {
                    horario.Disponibilidad = nuevaDisponibilidad;
                }
                return Page(); // Refresh the page to show updated availability
            }
            else
            {
                // Handle failure case
                ModelState.AddModelError("", "Failed to update availability.");
                return Page();
            }
        }

    }
}
