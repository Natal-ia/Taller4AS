using System.Net.Http;
using System.Net.Http.Json;
using RazorUI.Models;

namespace RazorUI.Services
{
   public class EspaciosService
{
    private readonly HttpClient _httpClient;

    public EspaciosService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EspacioViewModel>> GetEspaciosWithHorariosAsync()
    {
        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5242/");
        }

        var response = await _httpClient.GetFromJsonAsync<List<EspacioViewModel>>("Espacios/ListWithHorarios");
        return response ?? new List<EspacioViewModel>();
    }

    public async Task<bool> UpdateDisponibilidadAsync(int espacioId, int horarioId, bool nuevaDisponibilidad)
    {
        var response = await _httpClient.PutAsJsonAsync($"Espacios/UpdateDisponibilidad/{espacioId}/{horarioId}", nuevaDisponibilidad);
        return response.IsSuccessStatusCode; // Returns true if the update was successful
    }
}

}
