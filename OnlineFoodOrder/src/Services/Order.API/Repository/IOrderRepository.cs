using FoodOrder.Shared.Repository;

namespace Order.API.Repository
{
    public interface IOrderRepository : IRepository<Order.API.Entities.Order>
    {
        Task<int> CountTodayAsync(DateTime date);

        Task<Order.API.Entities.Order?> GetByIdWithDetailsAsync(Guid id);

        Task<IEnumerable<Order.API.Entities.Order>> GetByCustomerAsync(Guid customerId);

        Task<IEnumerable<Order.API.Entities.Order>> GetByRestaurantAsync(Guid restaurantId);

        Task<IEnumerable<Order.API.Entities.Order>> GetRecentAsync(int take);
    }
}
