namespace Delivery.API.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IDeliveryRepository Deliveries { get; }
        Task<int> SaveChangesAsync();
    }
}
