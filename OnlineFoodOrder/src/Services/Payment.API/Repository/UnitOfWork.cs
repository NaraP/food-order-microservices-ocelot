using Payment.API.Data;

namespace Payment.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PaymentDbContext _db;

        public IPaymentRepository Payments { get; }

        public UnitOfWork(PaymentDbContext db)
        {
            _db = db;
            Payments = new PaymentRepository(db);
        }

        public async Task<int> SaveChangesAsync() =>
            await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();
    }
}
