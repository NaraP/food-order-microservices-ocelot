using Order.API.Data;

namespace Order.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderDbContext _db;

        public IOrderRepository Orders { get; }

        public UnitOfWork(OrderDbContext db)
        {
            _db = db;
            Orders = new OrderRepository(db);
        }

        public async Task<int> SaveChangesAsync() =>
            await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();
    }
}
