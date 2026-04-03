using FoodOrder.Shared.Repository;

namespace Payment.API.Repository
{
    public interface IPaymentRepository : IRepository<Payment.API.Entities.Payment>
    {
        Task<Payment.API.Entities.Payment?> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Payment.API.Entities.Payment>> GetByCustomerAsync(Guid customerId);
    }
}
