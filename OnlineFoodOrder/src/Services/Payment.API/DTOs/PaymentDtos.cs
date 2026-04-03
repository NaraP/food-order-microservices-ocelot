// ── DTOs ──────────────────────────────────────────────────────────────────────
namespace Payment.API.DTOs;
using System.ComponentModel.DataAnnotations;
public record InitiatePaymentDto([Required] Guid OrderId, [Required] decimal Amount,
    [Required] string Method, [Required] string CustomerEmail);
public record PaymentDto(Guid Id, Guid OrderId, decimal Amount, string Method,
    string State, string TransactionId, DateTime CreatedAt, DateTime? CompletedAt);
public record RefundDto([Required] Guid PaymentId, [Required] string Reason);
