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

        public async Task<List<Order>> GetAllOrders()
        {
            return await _dbContext.tblOrders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderById(int Id)
        {
            return await _dbContext.tblOrders
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task UpdateOrderStatus(Order order)
        {
           _dbContext.tblOrders.Update(order);
            await _dbContext.SaveChangesAsync();
        }
    }
}
