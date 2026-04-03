// ── Entity ────────────────────────────────────────────────────────────────────
namespace Payment.API.Entities;

public enum PaymentState  { Pending=1, Processing=2, Completed=3, Failed=4, Refunded=5 }
public enum PaymentMethod { UPI=1, Card=2, NetBanking=3, Wallet=4, COD=5 }

public class Payment
{
    public Guid          Id            { get; set; } = Guid.NewGuid();
    public Guid          OrderId       { get; set; }
    public Guid          CustomerId    { get; set; }
    public string        CustomerEmail { get; set; } = string.Empty;
    public decimal       Amount        { get; set; }
    public PaymentMethod Method        { get; set; }
    public PaymentState  State         { get; set; } = PaymentState.Pending;
    public string        TransactionId { get; set; } = string.Empty;
    public string?       FailureReason { get; set; }
    public DateTime      CreatedAt     { get; set; } = DateTime.UtcNow;
    public DateTime?     CompletedAt   { get; set; }
}
