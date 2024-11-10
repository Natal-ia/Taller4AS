using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorUI.Models;
using RazorUI.Services;

namespace RazorUI.Pages
{
    public class EspaciosModel : PageModel
    {
        private readonly EspaciosService _espaciosService;

        public EspaciosModel(EspaciosService espaciosService)
        {
            _espaciosService = espaciosService;
        }

        public List<EspacioViewModel> Espacios { get; set; }

        public async Task OnGetAsync()
        {
            Espacios = await _espaciosService.GetEspaciosWithHorariosAsync();
        }

    }
}
