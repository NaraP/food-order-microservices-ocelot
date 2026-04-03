namespace Restaurant.API.Repository
{
    public interface IRestaurantRepository : IRepository<Restaurant.API.Entities.Restaurant>
    {
        Task<IEnumerable<Restaurant.API.Entities.Restaurant>> GetAllWithMenuAsync(string? city, string? cuisine);
        Task<Restaurant.API.Entities.Restaurant?> GetByIdWithMenuAsync(Guid id);
    }
}
