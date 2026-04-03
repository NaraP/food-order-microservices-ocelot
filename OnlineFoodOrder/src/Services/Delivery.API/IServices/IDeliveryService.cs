using Delivery.API.DTOs;

namespace Delivery.API.IServices
{
    public interface IDeliveryService
    {
        Task<DeliveryDto> AssignAsync(AssignDto dto);
        Task<DeliveryDto?> GetByOrderAsync(Guid orderId);
        Task<IEnumerable<DeliveryDto>> GetByDriverAsync(Guid driverId);
        Task<IEnumerable<DeliveryDto>> GetActiveAsync();
        Task<bool> UpdateStatusAsync(Guid id, UpdateStatusDto dto);
        Task<bool> UpdateLocationAsync(Guid id, UpdateLocationDto dto);
    }
}
