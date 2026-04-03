using Order.API.DTOs;

namespace Order.API.IServices
{
    public interface IOrderService
    {
        Task<OrderResponse> PlaceAsync(PlaceOrderDto dto, Guid customerId, string name, string email, string phone);
        Task<OrderResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderResponse>> GetMyOrdersAsync(Guid customerId);
        Task<IEnumerable<OrderResponse>> GetAllAsync();
        Task<IEnumerable<OrderResponse>> GetByRestaurantAsync(Guid restaurantId);
        Task<bool> UpdateStatusAsync(Guid id, UpdateStatusDto dto);
        Task<bool> CancelAsync(Guid id, Guid customerId, string reason);
        Task<bool> MarkPaidAsync(Guid id, string method);
    }
}
