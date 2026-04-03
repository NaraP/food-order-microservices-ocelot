using Payment.API.DTOs;

namespace Payment.API.IServices
{
    public interface IPaymentService
    {
        Task<PaymentDto> InitiateAsync(InitiatePaymentDto dto, Guid customerId);
        Task<PaymentDto?> GetByOrderAsync(Guid orderId);
        Task<IEnumerable<PaymentDto>> GetMyPaymentsAsync(Guid customerId);
        Task<bool> RefundAsync(RefundDto dto);
    }
}
