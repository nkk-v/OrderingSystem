using OrderingSystem.Data;
using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly AppDbContext _dbContext;

        public PaymentRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddPayment(Payment payment)
        {
            await _dbContext.tblPayments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();
        }
    }
}
