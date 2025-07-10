using Microsoft.AspNetCore.Mvc;
using OrderingSystem.DTO;
using OrderingSystem.Models;
using OrderingSystem.Services;

namespace OrderingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpPost("calculate-fee")]
        public async Task<IActionResult> CalculateFee([FromBody] AddressDTO dto)
        {
            var origin = new Coordinates { Latitude = 14.59907270949496, Longitude = 121.10925635666807 };
            var destination = await _deliveryService.GeoCodeCoordinate(dto.Address);

            if (destination == null) return BadRequest("Invalid address.");

            var estimate = await _deliveryService.CalculateDistance(origin, destination, dto.ItemCount);
            if (estimate == null) return BadRequest("Sorry, we can't deliver to your area because it's out of range.");

            return Ok(estimate);
        }

        [HttpGet("autocomplete")]
        public async Task<IActionResult> AutoComplete(string query)
        {
            var result = await _deliveryService.AutoCompleteAddress(query);

            if (result == null)
                return Ok(new { features = new List<object>() }); // always return a valid structure

            return Ok(result); // This returns valid JSON with correct structure
        }

        [HttpGet("reverse-geocode")]
        public async Task<IActionResult> ReverseGeocode([FromQuery] double lat, [FromQuery] double lon)
        {
            var address = await _deliveryService.ReverseGeocode(lat, lon);
            if (address == null) return BadRequest("Unable to reverse geocode coordinates");

            return Ok(new {address});
        }

    }
}
