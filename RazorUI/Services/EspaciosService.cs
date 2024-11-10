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
                //throw new InvalidOperationException("HttpClient BaseAddress is not set.");
            }

            // Print the BaseAddress
            Console.WriteLine($"BaseAddress: {_httpClient.BaseAddress}");

            // Make the request
            var response = await _httpClient.GetFromJsonAsync<List<EspacioViewModel>>("Espacios/ListWithHorarios");

            // Handle response and return data
            return response ?? new List<EspacioViewModel>(); 
        }
    }
}
