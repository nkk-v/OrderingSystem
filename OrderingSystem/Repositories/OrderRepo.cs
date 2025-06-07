using Microsoft.EntityFrameworkCore;
using OrderingSystem.Data;
using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _dbContext;

        public OrderRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddOrderItem(IEnumerable<OrderItem> orderItems)
        {
            _dbContext.tblOrderItems.AddRange(orderItems);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddtoOrder(Order order)
        {
            _dbContext.tblOrders.Add(order);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrders(string status)
        {
            //return await _dbContext.tblOrders
            //    .Include(o => o.OrderItems)
            //        .ThenInclude(oi => oi.Product)
            //    .OrderByDescending(x => x.OrderDate)
            //    .ToListAsync();
            var query =  _dbContext.tblOrders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(o => o.DeliveryStatus == status && o.OrderStatus == "Success");
            }
            else
            {
                query = query.Where(o => o.OrderStatus == "Success");
            }

            return await query.ToListAsync();
        }

        public async Task<List<Order>> GetLatestOrder()
        {
            return await _dbContext.tblOrders
                .Include(x => x.OrderItems)
                    .ThenInclude(x => x.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderById(int Id)
        {
            return await _dbContext.tblOrders
                .Include (o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)    
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

       
        public async Task<List<Order>> OrderHistoryByUser(string userId)
        {
            var query = _dbContext.tblOrders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.ProductVariant)
                .OrderByDescending(x => x.OrderNum)
                .AsQueryable();


            query = query.Where(x =>  x.UserId == userId && x.OrderStatus == "Success");


            return await query.ToListAsync();
        }

        public async Task UpdateStatus(Order order)
        {
           _dbContext.tblOrders.Update(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}
