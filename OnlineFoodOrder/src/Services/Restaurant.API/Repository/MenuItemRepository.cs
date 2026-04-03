using Restaurant.API.Data;
using Restaurant.API.Entities;

namespace Restaurant.API.Repository
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        public MenuItemRepository(RestaurantDbContext db) : base(db) { }
    }
}
