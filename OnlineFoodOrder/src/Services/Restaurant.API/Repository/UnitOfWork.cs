using Restaurant.API.Data;

namespace Restaurant.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestaurantDbContext _db;

        public IRestaurantRepository Restaurants { get; }
        public IMenuItemRepository MenuItems { get; }
        public ICategoryRepository Categories { get; }

        public UnitOfWork(RestaurantDbContext db)
        {
            _db = db;
            Restaurants = new RestaurantRepository(db);
            MenuItems = new MenuItemRepository(db);
            Categories = new CategoryRepository(db);
        }

        public async Task<int> SaveChangesAsync() =>
            await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();
    }
}
