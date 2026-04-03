using Identity.API.Data;
using Identity.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext db) : base(db) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<User?> GetByRefreshTokenAsync(string token)
        {
            return await _db.Users
                .FirstOrDefaultAsync(u =>
                    u.RefreshToken == token &&
                    u.RefreshTokenExpiry > DateTime.UtcNow &&
                    u.IsActive);
        }
    }
}
