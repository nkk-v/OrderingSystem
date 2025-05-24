using OrderingSystem.DTO;
using OrderingSystem.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OrderingSystem.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly string _apiKey;

        public DeliveryService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
            _apiKey = _config["ORS:ApiKey"];
        }

        public async Task<AutoCompleteResponseDTO?> AutoCompleteAddress(string query)
        {
            if (string.IsNullOrWhiteSpace(query) || query.Length < 3) return null;

            var url = $"https://api.openrouteservice.org/geocode/search?api_key={_apiKey}&text={Uri.EscapeDataString(query)}&boundary.country=PH&layers=address,venue,locality&sources=openstreetmap";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<AutoCompleteResponseDTO>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                return null;
            }

        }

        public async Task<double?> CalculateDistance(Coordinates origin, Coordinates destination)
        {
            var body = new
            {
                locations = new[]
                {
                    new[] { origin.Longitude, origin.Latitude },
                    new[] { destination.Longitude, destination.Latitude },
                },
                metrics = new[] { "distance" },
                units = "km"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openrouteservice.org/v2/matrix/driving-car")
            {
                Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json);

            return data.RootElement.GetProperty("distances")[0][1].GetDouble();
        }

        public async Task<Coordinates?> GeoCodeCoordinate(string address)
        {
           var url = $"https://api.openrouteservice.org/geocode/search?api_key={_apiKey}&text={Uri.EscapeDataString(address)}&boundary.country=PH&layers=address,venue,locality";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(json);
            var coor = data.RootElement
                           .GetProperty("features")[0]
                           .GetProperty("geometry")
                           .GetProperty("coordinates");

            return new Coordinates
            {
                Longitude = coor[0].GetDouble(),
                Latitude = coor[1].GetDouble()
            };
        }
    }
}
