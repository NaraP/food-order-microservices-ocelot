namespace FoodOrder.Web.Models
{
    // ── Payment ───────────────────────────────────────────────────────────────────
    public class PaymentVm
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
