using OrderingSystem.Models;
using OrderingSystem.ViewModels;
using RestSharp;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Options;

namespace OrderingSystem.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IDeliveryService _deliveryService;
        private readonly IConfiguration _config;
        private readonly string _secretKey;
        private readonly string _baseUrl;

        public CheckoutService(ICartService cartService, IOrderService orderService, IAccountService accountService, IDeliveryService deliveryService,
            IConfiguration config)
        {
            _cartService = cartService;
            _orderService = orderService;
            _accountService = accountService;
            _deliveryService = deliveryService;
            _config = config;
            _secretKey = _config["PayMongo:SecretKey"];
            _baseUrl = _config["PayMongo:BaseUrl"];
        }

        public async Task<string> CreatePaymentLinkAsync(CheckoutViewModel model, string userId)
        {
            var options = new RestClientOptions(_baseUrl);
            var client = new RestClient(options);
            var request = new RestRequest("", Method.Post);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_secretKey}:")));

            
            var orderId = await _orderService.AddOrder(model, userId);

            var metadata = new Dictionary<string, string>
            {
                { "UserId", userId.ToString() },
                { "OrderId", orderId.ToString() }
                
            };

            //await _cartService.ClearCartItems(userId);

            var body = new
            {
                data = new
                {
                    attributes = new
                    {
                        send_email_receipt = true,
                        show_description = true,
                        show_line_items = true,

                        description = await _orderService.GetLatestOrderByUser(userId),//$"Total items: {model.CartItems.Count}",
                        line_items = model.CartItems.Select(item => new
                        {
                            currency = "PHP",
                            amount = Convert.ToInt32(item.Price) * 100,
                            name = item.ProductName,
                            quantity = item.Quantity
                        })
                        .Append(new
                        {
                            currency = "PHP",
                            amount = Convert.ToInt32(model.DeliveryFee) * 100,
                            name = "Delivery Fee",
                            quantity = 1
                        }),
                        payment_method_types = new[] { "paymaya", "gcash", "qrph"},

                        success_url = "https://localhost:7248/Checkout/Success",
                        cancel_url = "https://localhost:7248/Checkout/Failed",
                        metadata = metadata

                    }
                }
            };

            request.AddJsonBody(body);
            var response = await client.ExecutePostAsync(request);

            if (!response.IsSuccessful)
            {
                Console.WriteLine($"❌ Status: {response.StatusCode}");
                Console.WriteLine($"❌ Response: {response.Content}");
                throw new HttpRequestException($"PayMongo request failed: {response.Content}");
            }

            var content = JsonDocument.Parse(response.Content!);

            // Extract payment intent or checkout session ID
            var checkoutSessionId = content.RootElement.GetProperty("data").GetProperty("id").GetString();

            var paymentIntentId = await GetPaymentIntentId(checkoutSessionId);

            await _orderService.UpdateRefNo(orderId, paymentIntentId);

            return content.RootElement.GetProperty("data").GetProperty("attributes").GetProperty("checkout_url").GetString();
        }

        public async Task<CheckoutViewModel> GetCheckoutViewModelAsync(string userId)
        {
            var user = await _accountService.GetUserDetails(userId);
            var cart = await _cartService.GetUserCart(userId);
            var origin = new Coordinates { Latitude = 14.59907270949496, Longitude = 121.10925635666807 };
            var destination = await _deliveryService.GeoCodeCoordinate(user.Address);

            var distance = await _deliveryService.CalculateDistance(origin, destination);
            var ratePerKm = 10;
            var fee = Math.Max(Math.Ceiling(distance.Value * ratePerKm), 50);

            return new CheckoutViewModel
            {
                

                Fullname = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                SubTotal = cart.CartItems.Sum(x => x.Price * x.Quantity),
                DeliveryFee = Convert.ToDecimal(fee),
                CartItems = cart.CartItems.Select(x => new CartItemViewModel
                {
                    Id = x.Id,
                    CartId = x.CartId,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    Price = x.Price

                }).ToList()
            };
        }

        public async Task<string> GetPaymentIntentId(string checkoutSessionId)
        {
            var options = new RestClientOptions(_baseUrl + $"/{checkoutSessionId}");
            var client = new RestClient(options);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_secretKey}:")));

            var response = await client.ExecuteAsync(request);
            if (!response.IsSuccessful)
                throw new Exception("Failed to fetch checkout session details.");

            var json = JsonDocument.Parse(response.Content!);
            var intentId = json.RootElement.GetProperty("data")
                                           .GetProperty("attributes")
                                           .GetProperty("payment_intent")
                                           .GetProperty("id")
                                           .GetString();

            return intentId;
        }
    }
}
