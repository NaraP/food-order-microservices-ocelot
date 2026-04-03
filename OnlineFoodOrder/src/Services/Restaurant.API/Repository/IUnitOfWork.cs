namespace Restaurant.API.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRestaurantRepository Restaurants { get; }
        IMenuItemRepository MenuItems { get; }
        ICategoryRepository Categories { get; }

        Task<int> SaveChangesAsync();
    }
}
