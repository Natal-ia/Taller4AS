using System.Net.Http;
using System.Net.Http.Json;
using RazorUI.Models;

namespace RazorUI.Services
{
    public class EspaciosService
    {
        private readonly HttpClient _httpClient;

        public EspaciosService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("EspaciosApi");
        }

     public async Task<List<EspacioViewModel>> GetEspaciosWithHorariosAsync()
{
    var response = await _httpClient.GetFromJsonAsync<List<EspacioViewModel>>("Espacios/ListWithHorarios");
    return response;
}


    }
}
