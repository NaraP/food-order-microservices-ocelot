using Delivery.API.Data;

namespace Delivery.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DeliveryDbContext _db; 

        public IDeliveryRepository Deliveries { get; }

        public UnitOfWork(DeliveryDbContext db)
        {
            _db = db;
            Deliveries = new DeliveryRepository(db);
        }

        public async Task<int> SaveChangesAsync() =>
            await _db.SaveChangesAsync();

        public void Dispose() =>
            _db.Dispose();
    }
}
