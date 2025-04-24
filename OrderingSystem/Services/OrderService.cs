using OrderingSystem.Models;
using OrderingSystem.Repositories;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;

        public OrderService(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<int> AddOrder(CheckoutViewModel model, string userId)
        {
            DateTime? scheduleDeliver = null;
            DateTime? orderDate = null;
            if (model.DeliveryOption == "later" && model.ScheduledDate.HasValue && !string.IsNullOrEmpty(model.ScheduledTime))
            {
                if (DateTime.TryParse($"{model.ScheduledDate.Value:yyyy-MM-dd} {model.ScheduledTime}", out var parsed))
                {
                    scheduleDeliver = parsed;
                }
            }
            else
            {
                orderDate = DateTime.Now;
            }

            var order = new Order
            {
                OrderNum = Guid.NewGuid(),
                UserId = userId,
                OrderDate = orderDate,
                TotalAmount = model.TotalAmout,
                Status = "Pending",
                ScheduledDate = scheduleDeliver,
                DeliveryNote = model.DeliveryNote,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address

            };

            await _orderRepo.AddtoOrder(order);


            var orderItems = model.CartItems.Select(item => new OrderItem
            {
                OrderId = order.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price,

            });

            await _orderRepo.AddOrderItem(orderItems);

            return order.Id;

        }
    }
}
