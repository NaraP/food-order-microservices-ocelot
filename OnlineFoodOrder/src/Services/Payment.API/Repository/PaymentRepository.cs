using Microsoft.EntityFrameworkCore;
using Payment.API.Data;

namespace Payment.API.Repository
{
    public class PaymentRepository : Repository<Payment.API.Entities.Payment>, IPaymentRepository
    {
        public PaymentRepository(PaymentDbContext db) : base(db) { }

        public async Task<Payment.API.Entities.Payment?> GetByOrderIdAsync(Guid orderId)
        {
            return await _db.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<IEnumerable<Payment.API.Entities.Payment>> GetByCustomerAsync(Guid customerId)
        {
            return await _db.Payments
                .Where(p => p.CustomerId == customerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}
