using Identity.API.Data;

namespace Identity.API.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public IUserRepository Users { get; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Users = new UserRepository(db);
        }

        public async Task<int> SaveChangesAsync() =>
            await _db.SaveChangesAsync();

        public void Dispose() => _db.Dispose();
    }
}
