using Microsoft.EntityFrameworkCore;
using Order.API.Data;

namespace Order.API.Repository
{
    public class OrderRepository : Repository<Order.API.Entities.Order>, IOrderRepository
    {
        public OrderRepository(OrderDbContext db) : base(db) { }

        public async Task<int> CountTodayAsync(DateTime date)
        {
            return await _db.Orders.CountAsync(o => o.CreatedAt.Date == date.Date);
        }

        public async Task<Order.API.Entities.Order?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _db.Orders
                .Include(x => x.Items)
                .Include(x => x.History)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Order.API.Entities.Order>> GetByCustomerAsync(Guid customerId)
        {
            return await _db.Orders
                .Include(x => x.Items)
                .Include(x => x.History)
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order.API.Entities.Order>> GetByRestaurantAsync(Guid restaurantId)
        {
            return await _db.Orders
                .Include(x => x.Items)
                .Include(x => x.History)
                .Where(o => o.RestaurantId == restaurantId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order.API.Entities.Order>> GetRecentAsync(int take)
        {
            return await _db.Orders
                .Include(x => x.Items)
                .Include(x => x.History)
                .OrderByDescending(o => o.CreatedAt)
                .Take(take)
                .ToListAsync();
        }
    }
}
