using Microsoft.EntityFrameworkCore;
using Restaurant.API.Data;

namespace Restaurant.API.Repository
{
    public class RestaurantRepository : Repository<Restaurant.API.Entities.Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(RestaurantDbContext db) : base(db) { }

        public async Task<IEnumerable<Restaurant.API.Entities.Restaurant>> GetAllWithMenuAsync(string? city, string? cuisine)
        {
            var q = _db.Restaurants
                .Include(r => r.Categories)
                .ThenInclude(c => c.MenuItems)
                .Where(r => r.IsActive);

            if (!string.IsNullOrEmpty(city))
                q = q.Where(r => r.City.Contains(city));

            if (!string.IsNullOrEmpty(cuisine))
                q = q.Where(r => r.Cuisine.Contains(cuisine));

            return await q.OrderByDescending(r => r.Rating).ToListAsync();
        }

        public async Task<Restaurant.API.Entities.Restaurant?> GetByIdWithMenuAsync(Guid id)
        {
            return await _db.Restaurants
                .Include(r => r.Categories)
                .ThenInclude(c => c.MenuItems)
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }
    }
}
