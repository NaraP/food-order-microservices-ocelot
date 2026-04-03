using Restaurant.API.Data;
using Restaurant.API.Entities;

namespace Restaurant.API.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(RestaurantDbContext db) : base(db) { }
    }
}
