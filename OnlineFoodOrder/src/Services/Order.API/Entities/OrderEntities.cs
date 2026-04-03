namespace Order.API.Entities;

public enum OrderStatus
{
    Pending = 1, Confirmed = 2, Preparing = 3,
    ReadyForPickup = 4, OutForDelivery = 5, Delivered = 6, Cancelled = 7
}

public enum PaymentStatus { Pending = 1, Paid = 2, Failed = 3, Refunded = 4 }

public class Order
{
    public Guid          Id             { get; set; } = Guid.NewGuid();
    public string        OrderNumber    { get; set; } = string.Empty;
    public Guid          CustomerId     { get; set; }
    public string        CustomerName   { get; set; } = string.Empty;
    public string        CustomerEmail  { get; set; } = string.Empty;
    public string        CustomerPhone  { get; set; } = string.Empty;
    public Guid          RestaurantId   { get; set; }
    public string        RestaurantName { get; set; } = string.Empty;
    public string        DeliveryAddress{ get; set; } = string.Empty;
    public OrderStatus   Status         { get; set; } = OrderStatus.Pending;
    public PaymentStatus PaymentStatus  { get; set; } = PaymentStatus.Pending;
    public string        PaymentMethod  { get; set; } = string.Empty;
    public decimal       SubTotal       { get; set; }
    public decimal       DeliveryFee    { get; set; }
    public decimal       Tax            { get; set; }
    public decimal       TotalAmount    { get; set; }
    public string?       Notes          { get; set; }
    public string?       CancelReason   { get; set; }
    public int           EstimatedMins  { get; set; } = 45;
    public DateTime      CreatedAt      { get; set; } = DateTime.UtcNow;
    public DateTime?     ConfirmedAt    { get; set; }
    public DateTime?     DeliveredAt    { get; set; }
    public DateTime?     UpdatedAt      { get; set; }
    public ICollection<OrderItem>          Items   { get; set; } = new List<OrderItem>();
    public ICollection<OrderStatusHistory> History { get; set; } = new List<OrderStatusHistory>();
}

public class OrderItem
{
    public Guid    Id           { get; set; } = Guid.NewGuid();
    public Guid    OrderId      { get; set; }
    public Order   Order        { get; set; } = null!;
    public Guid    MenuItemId   { get; set; }
    public string  Name         { get; set; } = string.Empty;
    public int     Quantity     { get; set; }
    public decimal UnitPrice    { get; set; }
    public decimal TotalPrice   { get; set; }
    public string? Instructions { get; set; }
}

public class OrderStatusHistory
{
    public int         Id        { get; set; }
    public Guid        OrderId   { get; set; }
    public Order       Order     { get; set; } = null!;
    public OrderStatus Status    { get; set; }
    public string      Note      { get; set; } = string.Empty;
    public DateTime    Timestamp { get; set; } = DateTime.UtcNow;
}
