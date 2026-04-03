namespace FoodOrder.Web.Models
{

    // ── Order ─────────────────────────────────────────────────────────────────────
    public class OrderVm
    {
        public Guid Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string RestaurantName { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal SubTotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Tax { get; set; }
        public decimal TotalAmount { get; set; }
        public int EstimatedMins { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public string? Notes { get; set; }
        public List<OrderItemVm> Items { get; set; } = new();
        public List<HistoryVm> History { get; set; } = new();
    }
}
