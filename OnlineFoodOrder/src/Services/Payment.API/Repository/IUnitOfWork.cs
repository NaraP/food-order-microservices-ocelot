namespace Payment.API.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IPaymentRepository Payments { get; }

        Task<int> SaveChangesAsync();
    }
}
