using Delivery.API.DTOs;
using Delivery.API.Entities;
using Delivery.API.IServices;
using Delivery.API.Repository;
using FoodOrder.Shared.Contracts;
using FoodOrder.Shared.Events;

namespace Delivery.API.Services;

public class DeliveryService : IDeliveryService
{
    private readonly IUnitOfWork _uow;
    private readonly IEventBus _bus;

    public DeliveryService(IUnitOfWork uow, IEventBus bus)
    {
        _uow = uow;
        _bus = bus;
    }

    public async Task<DeliveryDto> AssignAsync(AssignDto dto)
    {
        var d = new Entities.Delivery
        {
            OrderId = dto.OrderId,
            DriverId = dto.DriverId,
            DriverName = dto.DriverName,
            DriverPhone = dto.DriverPhone,
            PickupAddress = dto.PickupAddr,
            DropAddress = dto.DropAddr,
            CustomerEmail = dto.CustomerEmail,
            EstMins = dto.EstMins
        };

        await _uow.Deliveries.AddAsync(d);
        await _uow.SaveChangesAsync();

        await _bus.PublishAsync(new DeliveryAssignedEvent
        {
            DeliveryId = d.Id,
            OrderId = d.OrderId,
            DriverName = d.DriverName,
            DriverPhone = d.DriverPhone,
            CustomerEmail = d.CustomerEmail
        });

        return ToDto(d);
    }

    public async Task<DeliveryDto?> GetByOrderAsync(Guid orderId)
    {
        var d = await _uow.Deliveries.GetByOrderIdAsync(orderId);
        return d == null ? null : ToDto(d);
    }

    public async Task<IEnumerable<DeliveryDto>> GetByDriverAsync(Guid driverId)
    {
        var list = await _uow.Deliveries.GetByDriverAsync(driverId);
        return list.Select(ToDto);
    }

    public async Task<IEnumerable<DeliveryDto>> GetActiveAsync()
    {
        var list = await _uow.Deliveries.GetActiveAsync();
        return list.Select(ToDto);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, UpdateStatusDto dto)
    {
        var d = await _uow.Deliveries.GetByIdAsync(id);

        if (d == null || !Enum.TryParse(dto.Status, out DeliveryStatus status))
            return false;

        d.Status = status;
        d.Note = dto.Note;
        d.UpdatedAt = DateTime.UtcNow;

        if (status == DeliveryStatus.PickedUp)
            d.PickedUpAt = DateTime.UtcNow;

        if (status == DeliveryStatus.Delivered)
            d.DeliveredAt = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        if (status == DeliveryStatus.Delivered)
        {
            await _bus.PublishAsync(new DeliveryCompletedEvent
            {
                DeliveryId = d.Id,
                OrderId = d.OrderId,
                CustomerEmail = d.CustomerEmail
            });
        }

        return true;
    }

    public async Task<bool> UpdateLocationAsync(Guid id, UpdateLocationDto dto)
    {
        var d = await _uow.Deliveries.GetByIdAsync(id);
        if (d == null) return false;

        d.Lat = dto.Lat;
        d.Lng = dto.Lng;
        d.Note = dto.Note;
        d.UpdatedAt = DateTime.UtcNow;

        await _uow.SaveChangesAsync();
        return true;
    }

    // ✅ Clean manual mapper
    private static DeliveryDto ToDto(Entities.Delivery d) => new(
        d.Id,
        d.OrderId,
        d.DriverName,
        d.DriverPhone,
        d.PickupAddress,
        d.DropAddress,
        d.Status.ToString(),
        d.EstMins,
        d.AssignedAt,
        d.PickedUpAt,
        d.DeliveredAt,
        d.Note,
        d.Lat,
        d.Lng
    );
}
