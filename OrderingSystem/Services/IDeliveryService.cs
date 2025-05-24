using OrderingSystem.DTO;
using OrderingSystem.Models;

namespace OrderingSystem.Services
{
    public interface IDeliveryService
    {
        Task<Coordinates?> GeoCodeCoordinate(string address);
        Task<double?> CalculateDistance(Coordinates origin, Coordinates destination);
        Task<AutoCompleteResponseDTO> AutoCompleteAddress(string query);
    }
}
