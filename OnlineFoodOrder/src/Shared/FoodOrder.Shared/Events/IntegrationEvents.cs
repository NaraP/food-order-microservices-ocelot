namespace FoodOrder.Shared.Events;

public abstract class IntegrationEvent
{
    public Guid     EventId    { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

// ── Order ─────────────────────────────────────────────────────────────────────
public class OrderPlacedEvent : IntegrationEvent
{
    public Guid    OrderId       { get; init; }
    public Guid    CustomerId    { get; init; }
    public string  CustomerEmail { get; init; } = string.Empty;
    public string  CustomerName  { get; init; } = string.Empty;
    public Guid    RestaurantId  { get; init; }
    public string  RestaurantName{ get; init; } = string.Empty;
    public decimal TotalAmount   { get; init; }
    public List<OrderItemEvent> Items { get; init; } = new();
}

public class OrderStatusChangedEvent : IntegrationEvent
{
    public Guid   OrderId       { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
    public string NewStatus     { get; init; } = string.Empty;
    public string Note          { get; init; } = string.Empty;
}

public class OrderCancelledEvent : IntegrationEvent
{
    public Guid   OrderId       { get; init; }
    public Guid   CustomerId    { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
    public string Reason        { get; init; } = string.Empty;
}

public class OrderItemEvent
{
    public string  Name      { get; init; } = string.Empty;
    public int     Quantity  { get; init; }
    public decimal UnitPrice { get; init; }
}

// ── Payment ───────────────────────────────────────────────────────────────────
public class PaymentSucceededEvent : IntegrationEvent
{
    public Guid    PaymentId     { get; init; }
    public Guid    OrderId       { get; init; }
    public string  CustomerEmail { get; init; } = string.Empty;
    public decimal Amount        { get; init; }
    public string  Method        { get; init; } = string.Empty;
    public string  TransactionId { get; init; } = string.Empty;
}

public class PaymentFailedEvent : IntegrationEvent
{
    public Guid   PaymentId     { get; init; }
    public Guid   OrderId       { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
    public string Reason        { get; init; } = string.Empty;
}

// ── Delivery ──────────────────────────────────────────────────────────────────
public class DeliveryAssignedEvent : IntegrationEvent
{
    public Guid   DeliveryId    { get; init; }
    public Guid   OrderId       { get; init; }
    public string DriverName    { get; init; } = string.Empty;
    public string DriverPhone   { get; init; } = string.Empty;
    public string CustomerEmail { get; init; } = string.Empty;
}

public class DeliveryCompletedEvent : IntegrationEvent
{
    public Guid   DeliveryId    { get; init; }
    public Guid   OrderId       { get; init; }
    public string CustomerEmail { get; init; } = string.Empty;
}
