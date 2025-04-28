using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<string> GenerateOrderNumber()
        {
            string datePart = DateTime.Now.ToString("yyMMdd");

            var latestOR = (await _orderRepo.GetLatestOrder())
                .Where(x => x.OrderNum.StartsWith($"ORD-{datePart}"))
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefault();

            int newSequence = 1;
            if(latestOR != null)
            {
                string lastSequence = latestOR.OrderNum.Substring(datePart.Length + 5);
                newSequence = int.Parse(lastSequence) + 1;
            }


            return $"ORD-{datePart}-{newSequence:D4}";
        }

        public async Task<string> AddOrder(CheckoutViewModel model, string userId)
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

            string newOrderNumber = await GenerateOrderNumber();


            var order = new Order
            {
                OrderNum = newOrderNumber,
                UserId = userId,
                OrderDate = orderDate,
                TotalAmount = model.TotalAmout,
                Status = "Pending",
                ScheduledDate = scheduleDeliver,
                DeliveryNote = model.DeliveryNote,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                fullname = model.Fullname,
                DateCreated = DateTime.Now

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

            return order.OrderNum;

        }

        public async Task<List<OrderViewModel>> GetAllOrders(string status)
        {
            var order = await _orderRepo.GetAllOrders(status);

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
