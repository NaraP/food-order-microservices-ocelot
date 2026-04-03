using FoodOrder.Shared.Repository;

namespace Delivery.API.Repository
{
    public interface IDeliveryRepository : IRepository<Delivery.API.Entities.Delivery>
    {
        Task<Delivery.API.Entities.Delivery?> GetByOrderIdAsync(Guid orderId);
        Task<IEnumerable<Delivery.API.Entities.Delivery>> GetByDriverAsync(Guid driverId);
        Task<IEnumerable<Delivery.API.Entities.Delivery>> GetActiveAsync();
    }
}
