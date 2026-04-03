// ── DTOs ──────────────────────────────────────────────────────────────────────
namespace Order.API.DTOs;
using System.ComponentModel.DataAnnotations;

public record PlaceOrderDto(
    [Required] Guid   RestaurantId,
    [Required] string RestaurantName,
    [Required] string DeliveryAddress,
    [Required] string PaymentMethod,
    [Required] List<OrderItemDto> Items,
    decimal DeliveryFee = 0,
    string? Notes = null);

public record OrderItemDto(
    [Required] Guid    MenuItemId,
    [Required] string  Name,
    [Required] int     Quantity,
    [Required] decimal UnitPrice,
    string? Instructions = null);

public record UpdateStatusDto([Required] string Status, string Note = "");

public record OrderResponse(
    Guid   Id, string OrderNumber, string RestaurantName, string DeliveryAddress,
    string Status, string PaymentStatus, string PaymentMethod,
    decimal SubTotal, decimal DeliveryFee, decimal Tax, decimal TotalAmount,
    int EstimatedMins, DateTime CreatedAt, DateTime? DeliveredAt, string? Notes,
    List<ItemResponse> Items, List<HistoryResponse> History);

public record ItemResponse(Guid MenuItemId, string Name, int Quantity, decimal UnitPrice, decimal TotalPrice, string? Instructions);
public record HistoryResponse(string Status, string Note, DateTime Timestamp);
