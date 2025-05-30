﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OrderingSystem.Models;
using OrderingSystem.Services;

namespace OrderingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly UserManager<User> _userManager;

        public WebhookController(IPaymentService paymentService, IOrderService orderService, ICartService cartService, UserManager<User> userManager)
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _cartService = cartService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();


            try
            {
                var webhook = JsonConvert.DeserializeObject<PayMongoWebhookEvent>(body);

                var eventType = webhook?.Data.Attributes.Type;


                switch (eventType)
                {
                    case "checkout_session.payment.paid":
                        var userId = webhook?.Data?.Attributes?.Data?.Attributes?.Metadata["UserId"];
                        var orderId = webhook?.Data?.Attributes?.Data?.Attributes?.Metadata["OrderId"];

                        var refNo = webhook?.Data?.Attributes?.Data?.Attributes?.payments[0].Id;

                        await _paymentService.SavePayment(webhook, int.Parse(orderId));
                        await _orderService.UpdateOrderStatus(int.Parse(orderId), eventType, refNo);
                        break;
                    case "payment.failed":
                        var failRefNo = webhook?.Data?.Attributes?.Data.Id;
                        var intentId = webhook?.Data?.Attributes?.Data?.Attributes?.payment_intent_id;

                        var FailedOrderId = await _orderService.GetLatestOrderId(intentId);
                        
                        await _orderService.UpdateOrderStatus(FailedOrderId, eventType, failRefNo);
                        break;
                    //case "payment.expired":
                    //    await _orderService.UpdateOrderStatus(int.Parse(orderId), eventType, refNo);
                    //    break;
                    default:
                        break;
                }


                return Ok(); // Send 200 back to PayMongo
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Failed to process webhook:");
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
    }
}
