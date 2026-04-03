namespace Delivery.API.Entities;

public enum DeliveryStatus { Assigned=1, PickedUp=2, InTransit=3, Delivered=4, Failed=5 }

public class Delivery
{
    public Guid           Id            { get; set; } = Guid.NewGuid();
    public Guid           OrderId       { get; set; }
    public Guid           DriverId      { get; set; }
    public string         DriverName    { get; set; } = string.Empty;
    public string         DriverPhone   { get; set; } = string.Empty;
    public string         PickupAddress { get; set; } = string.Empty;
    public string         DropAddress   { get; set; } = string.Empty;
    public string         CustomerEmail { get; set; } = string.Empty;
    public DeliveryStatus Status        { get; set; } = DeliveryStatus.Assigned;
    public string?        Note          { get; set; }
    public double?        Lat           { get; set; }
    public double?        Lng           { get; set; }
    public int            EstMins       { get; set; } = 20;
    public DateTime       AssignedAt    { get; set; } = DateTime.UtcNow;
    public DateTime?      PickedUpAt    { get; set; }
    public DateTime?      DeliveredAt   { get; set; }
    public DateTime?      UpdatedAt     { get; set; }
}
