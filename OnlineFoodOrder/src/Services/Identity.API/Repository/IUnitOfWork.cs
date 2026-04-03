namespace Identity.API.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }

        Task<int> SaveChangesAsync();
    }
}
