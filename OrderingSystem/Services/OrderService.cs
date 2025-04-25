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
                Address = model.Address,
                fullname = model.Fullname
                

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

        public async Task<List<OrderViewModel>> GetAllOrders()
        {
            var order = await _orderRepo.GetAllOrders();

            return order.Select(o => new OrderViewModel
            {
                Id = o.Id,
                UserId = o.UserId,
                OrderNum = o.OrderNum,
                DeliveryDate = o.OrderDate ?? o.ScheduledDate,
                ContactNumber = o.PhoneNumber,
                Address = o.Address,
                DeliveryNote = o.DeliveryNote,
                TotalAmount = o.TotalAmount, 
                Status = o.Status,
                Fullname = o.fullname,
                OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity
                }).ToList()

            }).ToList();
           
            

        }

        public async Task UpdateOrderStatus(int OrderId, string newStatus)
        {
            var order = await _orderRepo.GetOrderById(OrderId);

            if(order != null)
            {
                order.Status = newStatus;
                await _orderRepo.UpdateOrderStatus(order);
            }



        }
    }
}
