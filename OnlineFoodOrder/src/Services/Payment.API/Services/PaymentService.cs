using Payment.API.DTOs;
using Payment.API.Entities;
using FoodOrder.Shared.Contracts;
using FoodOrder.Shared.Events;
using Payment.API.IServices;
using Payment.API.Repository;

namespace Payment.API.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IEventBus _bus;

    public PaymentService(IUnitOfWork uow, IEventBus bus)
    {
        _uow = uow;
        _bus = bus;
    }

    public async Task<PaymentDto> InitiateAsync(InitiatePaymentDto dto, Guid customerId)
    {
        if (!Enum.TryParse<PaymentMethod>(dto.Method, out var method))
            method = PaymentMethod.UPI;

        var p = new Entities.Payment
        {
            OrderId = dto.OrderId,
            CustomerId = customerId,
            CustomerEmail = dto.CustomerEmail,
            Amount = dto.Amount,
            Method = method,
            State = PaymentState.Processing,
            TransactionId = $"TXN-{method.ToString().ToUpper()[..3]}-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}"
        };

        await _uow.Payments.AddAsync(p);
        await _uow.SaveChangesAsync();

        // Simulate gateway
        await Task.Delay(50);

        p.State = method == PaymentMethod.COD
            ? PaymentState.Pending
            : PaymentState.Completed;

        p.CompletedAt = p.State == PaymentState.Completed
            ? DateTime.UtcNow
            : null;

        await _uow.SaveChangesAsync();

        if (p.State == PaymentState.Completed)
        {
            await _bus.PublishAsync(new PaymentSucceededEvent
            {
                PaymentId = p.Id,
                OrderId = dto.OrderId,
                CustomerEmail = dto.CustomerEmail,
                Amount = dto.Amount,
                Method = dto.Method,
                TransactionId = p.TransactionId
            });
        }

        return ToDto(p);
    }

    public async Task<PaymentDto?> GetByOrderAsync(Guid orderId)
    {
        var p = await _uow.Payments.GetByOrderIdAsync(orderId);
        return p == null ? null : ToDto(p);
    }

    public async Task<IEnumerable<PaymentDto>> GetMyPaymentsAsync(Guid customerId)
    {
        var list = await _uow.Payments.GetByCustomerAsync(customerId);
        return list.Select(ToDto);
    }

    public async Task<bool> RefundAsync(RefundDto dto)
    {
        var p = await _uow.Payments.GetByIdAsync(dto.PaymentId);

        if (p == null || p.State != PaymentState.Completed)
            return false;

        p.State = PaymentState.Refunded;
        await _uow.SaveChangesAsync();

        return true;
    }

    // ✅ Manual mapping (best for microservices)
    private static PaymentDto ToDto(Entities.Payment p) =>
        new(
            p.Id,
            p.OrderId,
            p.Amount,
            p.Method.ToString(),
            p.State.ToString(),
            p.TransactionId,
            p.CreatedAt,
            p.CompletedAt
        );
}
