// ── DTOs ──────────────────────────────────────────────────────────────────────
namespace Delivery.API.DTOs;
using System.ComponentModel.DataAnnotations;

public record AssignDto(
    [Required] Guid   OrderId,     [Required] Guid   DriverId,
    [Required] string DriverName,  [Required] string DriverPhone,
    [Required] string PickupAddr,  [Required] string DropAddr,
    [Required] string CustomerEmail, int EstMins = 20);

public record UpdateStatusDto([Required] string Status, string? Note = null);
public record UpdateLocationDto(double Lat, double Lng, string? Note = null);

public record DeliveryDto(
    Guid Id, Guid OrderId, string DriverName, string DriverPhone,
    string PickupAddr, string DropAddr, string Status, int EstMins,
    DateTime AssignedAt, DateTime? PickedUpAt, DateTime? DeliveredAt,
    string? Note, double? Lat, double? Lng);
