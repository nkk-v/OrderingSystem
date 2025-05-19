using OrderingSystem.Models;
using OrderingSystem.ViewModels;
using RestSharp;
using System.Text.Json;
using System.Text;

namespace OrderingSystem.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IConfiguration _config;
        private readonly string _secretKey;
        private readonly string _baseUrl;

        public CheckoutService(ICartService cartService, IOrderService orderService, IAccountService accountService, IConfiguration config)
        {
            _cartService = cartService;
            _orderService = orderService;
            _accountService = accountService;
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


            var body = new
            {
                data = new
                {
                    attributes = new
                    {
                        send_email_receipt = true,
                        show_description = true,
                        show_line_items = true,

                        description = $"Total items: {model.CartItems.Count}",
                        line_items = model.CartItems.Select(item => new
                        {
                            currency = "PHP",
                            amount = Convert.ToInt32(item.Price) * 100,
                            name = item.ProductName,
                            quantity = item.Quantity
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
            return content.RootElement.GetProperty("data").GetProperty("attributes").GetProperty("checkout_url").GetString();
        }

        public async Task<CheckoutViewModel> GetCheckoutViewModelAsync(string userId)
        {
            var user = await _accountService.GetUserDetails(userId);
            var cart = await _cartService.GetUserCart(userId);

            return new CheckoutViewModel
            {
                Fullname = user.Fullname,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
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
    }
}
