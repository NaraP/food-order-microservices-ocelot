using Delivery.API.Data;
using Delivery.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Repository
{
    public class DeliveryRepository : Repository<Delivery.API.Entities.Delivery>, IDeliveryRepository
    {
        public DeliveryRepository(DeliveryDbContext db) : base(db) { }

        public async Task<Delivery.API.Entities.Delivery?> GetByOrderIdAsync(Guid orderId) =>
            await _db.Deliveries.FirstOrDefaultAsync(x => x.OrderId == orderId);

        public async Task<IEnumerable<Delivery.API.Entities.Delivery>> GetByDriverAsync(Guid driverId) =>
            await _db.Deliveries
                .Where(d => d.DriverId == driverId)
                .OrderByDescending(d => d.AssignedAt)
                .ToListAsync();

        public async Task<IEnumerable<Delivery.API.Entities.Delivery>> GetActiveAsync() =>
            await _db.Deliveries
                .Where(d => d.Status != DeliveryStatus.Delivered &&
                            d.Status != DeliveryStatus.Failed)
                .ToListAsync();
    }
}
