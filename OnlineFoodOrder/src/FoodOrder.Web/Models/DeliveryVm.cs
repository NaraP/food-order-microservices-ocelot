namespace FoodOrder.Web.Models
{

    // ── Delivery ──────────────────────────────────────────────────────────────────
    public class DeliveryVm
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string DriverPhone { get; set; } = string.Empty;
        public string PickupAddr { get; set; } = string.Empty;
        public string DropAddr { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int EstMins { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? PickedUpAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? Note { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
    }

}
