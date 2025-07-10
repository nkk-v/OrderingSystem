using OrderingSystem.DTO;
using OrderingSystem.Models;

namespace OrderingSystem.Services
{
    public interface IDeliveryService
    {
        Task<Coordinates?> GeoCodeCoordinate(string address);
        Task<DeliveryEstimateDTO?> CalculateDistance(Coordinates origin, Coordinates destination, int itemCount);
        Task<AutoCompleteResponseDTO> AutoCompleteAddress(string query);
        Task<string?> ReverseGeocode(double latitude, double longitude);
    }
}
