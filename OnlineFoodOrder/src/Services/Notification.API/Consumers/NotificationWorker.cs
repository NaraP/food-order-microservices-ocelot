using FoodOrder.Shared.Contracts;
using FoodOrder.Shared.Events;
using Microsoft.EntityFrameworkCore;
using Notification.API.Data;
using Notification.API.Entities;

namespace Notification.API.Consumers;

public class NotificationWorker : BackgroundService
{
    private readonly ILogger<NotificationWorker> _log;
    private readonly IServiceScopeFactory        _scope;

    public NotificationWorker(ILogger<NotificationWorker> log, IServiceScopeFactory scope)
    {
        _log   = log;
        _scope = scope;

        // Subscribe to all integration events
        InMemoryEventBus.Subscribe<OrderPlacedEvent>(e =>
            Save("OrderPlaced", e.CustomerEmail,
                $"Order #{e.OrderId} placed at {e.RestaurantName}",
                $"Hi {e.CustomerName}, your order of ₹{e.TotalAmount:N2} has been placed. We're waiting for restaurant confirmation.",
                e.OrderId));

        InMemoryEventBus.Subscribe<OrderStatusChangedEvent>(e =>
            Save("OrderStatus", e.CustomerEmail,
                $"Order status updated: {e.NewStatus}",
                $"Your order status is now '{e.NewStatus}'. {e.Note}",
                e.OrderId));

        InMemoryEventBus.Subscribe<OrderCancelledEvent>(e =>
            Save("OrderCancelled", e.CustomerEmail,
                "Your order has been cancelled",
                $"Your order has been cancelled. Reason: {e.Reason}. A refund will be processed if payment was made.",
                e.OrderId));

        InMemoryEventBus.Subscribe<PaymentSucceededEvent>(e =>
            Save("PaymentSuccess", e.CustomerEmail,
                $"Payment of ₹{e.Amount:N2} confirmed",
                $"Payment via {e.Method} successful. Transaction: {e.TransactionId}.",
                e.OrderId));

        InMemoryEventBus.Subscribe<PaymentFailedEvent>(e =>
            Save("PaymentFailed", e.CustomerEmail,
                "Payment failed",
                $"Your payment failed. Reason: {e.Reason}. Please retry.",
                e.OrderId));

        InMemoryEventBus.Subscribe<DeliveryAssignedEvent>(e =>
            Save("DeliveryAssigned", e.CustomerEmail,
                "Driver assigned to your order!",
                $"Your food is on its way! Driver: {e.DriverName} ({e.DriverPhone}). Track your order in the app.",
                e.OrderId));

        InMemoryEventBus.Subscribe<DeliveryCompletedEvent>(e =>
            Save("DeliveryCompleted", e.CustomerEmail,
                "Your order has been delivered!",
                "Your food has been delivered. Enjoy your meal! Please rate your experience.",
                e.OrderId));
    }

    protected override Task ExecuteAsync(CancellationToken ct)
    {
        _log.LogInformation("[NotificationWorker] Listening for events…");
        return Task.CompletedTask;
    }

    private async Task Save(string type, string email, string subject, string body, Guid? relatedId)
    {
        _log.LogInformation("[Notification] {Type} → {Email}", type, email);
        try
        {
            using var scope = _scope.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
            db.Logs.Add(new NotificationLog
            {
                EventType = type, Email = email,
                Subject   = subject, Body = body,
                Sent = true, RelatedId = relatedId, CreatedAt = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to save notification log");
        }
    }
}
