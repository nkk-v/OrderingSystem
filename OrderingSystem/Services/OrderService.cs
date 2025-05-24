using Microsoft.AspNetCore.Mvc.Rendering;
using OrderingSystem.Models;
using OrderingSystem.Repositories;
using OrderingSystem.ViewModels;

namespace OrderingSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ICartRepo _cartRepo;

        public OrderService(IOrderRepo orderRepo, ICartRepo cartRepo)
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
        }

        public async Task<string> GenerateOrderNumber()
        {
            var latestOR = (await _orderRepo.GetLatestOrder())
                .Where(x => x.OrderNum.StartsWith("OR"))
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefault();

            int newSequence = 1;
            if (latestOR != null)
            {
                string lastSequence = latestOR.OrderNum.Substring(2);
                newSequence = int.Parse(lastSequence) + 1;
            }

            return $"OR{newSequence:D13}";
            //string datePart = DateTime.Now.ToString("yyMMdd");

            //var latestOR = (await _orderRepo.GetLatestOrder())
            //    .Where(x => x.OrderNum.StartsWith($"ORD-{datePart}"))
            //    .OrderByDescending(x => x.DateCreated)
            //    .FirstOrDefault();

            //int newSequence = 1;
            //if(latestOR != null)
            //{
            //    string lastSequence = latestOR.OrderNum.Substring(datePart.Length + 5);
            //    newSequence = int.Parse(lastSequence) + 1;
            //}


            //return $"ORD-{datePart}-{newSequence:D4}";
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

            //var payment = webHook.Data.Attributes.Data.Attributes;

            string newOrderNumber = await GenerateOrderNumber();

            //var metaData = payment.Metadata;

            //DateTime.TryParse(metaData.GetValueOrDefault("OrderDate"), out var orderDate);
            //DateTime.TryParse(metaData.GetValueOrDefault("ScheduleDate"), out var scheduleDeliver);

            var order = new Order
            {
                OrderNum = newOrderNumber,
                UserId = userId,
                OrderDate = orderDate,
                TotalAmount = model.TotalAmount,
                SubTotal = model.SubTotal,
                DeliveryFee = model.DeliveryFee,
                DeliveryStatus = "Pending",
                ScheduledDate = scheduleDeliver,
                DeliveryNote = model.DeliveryNote,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                fullname = model.Fullname,
                OrderStatus = "Pending",
                DateCreated = DateTime.Now

            };

            await _orderRepo.AddtoOrder(order);


            //var cartItems = await _cartRepo.GetUserCart(userId);

            //var orderItems = cartItems.CartItems.Select(item => new OrderItem
            //{
            //    OrderId = order.Id,
            //    ProductId = item.ProductId,
            //    Quantity = item.Quantity,
            //    Price = item.Price,
            //});
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
                SubTotal = o.SubTotal, 
                DeliveryStatus = o.DeliveryStatus,
                Fullname = o.fullname,
                OrderItems = o.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity
                }).ToList()

        }).ToList();
           
            

        }

        public async Task UpdateDeliveryStatus(int OrderId, string newStatus)
        {
            var order = await _orderRepo.GetOrderById(OrderId);

            if(order != null)
            {
                order.DeliveryStatus = newStatus;
                await _orderRepo.UpdateStatus(order);
            }


        }

        public async Task<string> GetLatestOrderByUser(string userId)
        {
            var latestOrNo = (await _orderRepo.GetLatestOrder())
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefault();

            return latestOrNo.OrderNum;
        }

        public async Task UpdateOrderStatus(int OrderId, string eventType, string RefNo)
        {
            var order = await _orderRepo.GetOrderById(OrderId);
            if(order != null)
            {
                switch (eventType)
                {
                    case "checkout_session.payment.paid":
                        order.OrderStatus = "Success";
                        break;
                    case "payment.failed":
                        order.OrderStatus = "Failed";
                        break;
                    //case "payment.expired":
                    //    order.DeliveryStatus = "Expired";
                    //    break;
                    default:
                        break;
                }

                order.RefNo = RefNo;
                await _orderRepo.UpdateStatus(order);
            }
        }

        public async Task<int> GetLatestOrderId(string paymentId = "", string userId = "")
        {
            var order = (await _orderRepo.GetLatestOrder())
                .Where(x => x.RefNo == paymentId || x.UserId == userId)
                .OrderByDescending(x => x.DateCreated)
                .FirstOrDefault();

            return order.Id;
        }

        public async Task UpdateRefNo(int OrderId, string refNo)
        {
            var order = await _orderRepo.GetOrderById(OrderId);
            if(order != null)
            {
                order.RefNo = refNo;
                await _orderRepo.UpdateStatus(order);
            }
        }
    }
}
