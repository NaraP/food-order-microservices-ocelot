using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.DTOs;
using Order.API.Entities;
using FoodOrder.Shared.Contracts;
using FoodOrder.Shared.Events;
using Order.API.IServices;
using Order.API.Repository;

namespace Order.API.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _uow;
    private readonly IEventBus _bus;

    public OrderService(IUnitOfWork uow, IEventBus bus)
    {
        _uow = uow;
        _bus = bus;
    }

    public async Task<OrderResponse> PlaceAsync(PlaceOrderDto dto, Guid customerId,
        string name, string email, string phone)
    {
        var sub = dto.Items.Sum(i => i.UnitPrice * i.Quantity);
        var tax = Math.Round(sub * 0.05m, 2);
        var total = sub + dto.DeliveryFee + tax;
        var now = DateTime.UtcNow;

        var count = await _uow.Orders.CountTodayAsync(now);

        var order = new Entities.Order
        {
            OrderNumber = $"ORD-{now:yyyyMMdd}-{count + 1:D4}",
            CustomerId = customerId,
            CustomerName = name,
            CustomerEmail = email,
            CustomerPhone = phone,
            RestaurantId = dto.RestaurantId,
            RestaurantName = dto.RestaurantName,
            DeliveryAddress = dto.DeliveryAddress,
            PaymentMethod = dto.PaymentMethod,
            SubTotal = sub,
            DeliveryFee = dto.DeliveryFee,
            Tax = tax,
            TotalAmount = total,
            Notes = dto.Notes,
            Items = dto.Items.Select(i => new OrderItem
            {
                MenuItemId = i.MenuItemId,
                Name = i.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.UnitPrice * i.Quantity,
                Instructions = i.Instructions
            }).ToList(),
            History = new List<OrderStatusHistory>
            {
                new() { Status = OrderStatus.Pending, Note = "Order placed successfully", Timestamp = now }
            }
        };

        await _uow.Orders.AddAsync(order);
        await _uow.SaveChangesAsync();

        await _bus.PublishAsync(new OrderPlacedEvent
        {
            OrderId = order.Id,
            CustomerId = customerId,
            CustomerEmail = email,
            CustomerName = name,
            RestaurantId = dto.RestaurantId,
            RestaurantName = dto.RestaurantName,
            TotalAmount = total,
            Items = dto.Items.Select(i => new OrderItemEvent
            {
                Name = i.Name,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        });

        return (await GetByIdAsync(order.Id))!;
    }

    public async Task<OrderResponse?> GetByIdAsync(Guid id)
    {
        var o = await _uow.Orders.GetByIdWithDetailsAsync(id);
        return o == null ? null : Map(o);
    }

    public async Task<IEnumerable<OrderResponse>> GetMyOrdersAsync(Guid customerId)
    {
        var list = await _uow.Orders.GetByCustomerAsync(customerId);
        return list.Select(Map);
    }

    public async Task<IEnumerable<OrderResponse>> GetAllAsync()
    {
        var list = await _uow.Orders.GetRecentAsync(200);
        return list.Select(Map);
    }

    public async Task<IEnumerable<OrderResponse>> GetByRestaurantAsync(Guid restaurantId)
    {
        var list = await _uow.Orders.GetByRestaurantAsync(restaurantId);
        return list.Select(Map);
    }

    public async Task<bool> UpdateStatusAsync(Guid id, UpdateStatusDto dto)
    {
        var o = await _uow.Orders.GetByIdWithDetailsAsync(id);

        if (o == null || !Enum.TryParse(dto.Status, out OrderStatus status))
            return false;

        o.Status = status;
        o.UpdatedAt = DateTime.UtcNow;

        if (status == OrderStatus.Confirmed) o.ConfirmedAt = DateTime.UtcNow;
        if (status == OrderStatus.Delivered) o.DeliveredAt = DateTime.UtcNow;

        o.History.Add(new()
        {
            Status = status,
            Note = string.IsNullOrEmpty(dto.Note) ? status.ToString() : dto.Note
        });

        await _uow.SaveChangesAsync();

        await _bus.PublishAsync(new OrderStatusChangedEvent
        {
            OrderId = id,
            CustomerEmail = o.CustomerEmail,
            NewStatus = status.ToString(),
            Note = dto.Note
        });

        return true;
    }

    public async Task<bool> CancelAsync(Guid id, Guid customerId, string reason)
    {
        var list = await _uow.Orders.FindAsync(o => o.Id == id && o.CustomerId == customerId);
        var o = list.FirstOrDefault();

        if (o == null || o.Status >= OrderStatus.Preparing)
            return false;

        o.Status = OrderStatus.Cancelled;
        o.CancelReason = reason;
        o.UpdatedAt = DateTime.UtcNow;

        await _uow.SaveChangesAsync();

        await _bus.PublishAsync(new OrderCancelledEvent
        {
            OrderId = id,
            CustomerId = customerId,
            CustomerEmail = o.CustomerEmail,
            Reason = reason
        });

        return true;
    }

    public async Task<bool> MarkPaidAsync(Guid id, string method)
    {
        var o = await _uow.Orders.GetByIdAsync(id);
        if (o == null) return false;

        o.PaymentStatus = PaymentStatus.Paid;
        o.PaymentMethod = method;
        o.UpdatedAt = DateTime.UtcNow;

        await _uow.SaveChangesAsync();
        return true;
    }

    private static OrderResponse Map(Entities.Order o) => new(
        o.Id, o.OrderNumber, o.RestaurantName, o.DeliveryAddress,
        o.Status.ToString(), o.PaymentStatus.ToString(), o.PaymentMethod,
        o.SubTotal, o.DeliveryFee, o.Tax, o.TotalAmount, o.EstimatedMins,
        o.CreatedAt, o.DeliveredAt, o.Notes,
        o.Items.Select(i => new ItemResponse(i.MenuItemId, i.Name, i.Quantity, i.UnitPrice, i.TotalPrice, i.Instructions)).ToList(),
        o.History.OrderBy(h => h.Timestamp)
            .Select(h => new HistoryResponse(h.Status.ToString(), h.Note, h.Timestamp)).ToList()
    );
}
