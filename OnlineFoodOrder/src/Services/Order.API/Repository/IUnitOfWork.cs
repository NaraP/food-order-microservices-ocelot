namespace Order.API.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IOrderRepository Orders { get; }

        Task<int> SaveChangesAsync();
    }
}
