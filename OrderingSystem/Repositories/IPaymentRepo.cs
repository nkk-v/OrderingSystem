using OrderingSystem.Models;

namespace OrderingSystem.Repositories
{
    public interface IPaymentRepo
    {
        Task AddPayment(Payment payment);
    }
}
