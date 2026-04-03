using FoodOrder.Shared.Repository;
using Identity.API.Entities;

namespace Identity.API.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByRefreshTokenAsync(string token);
    }
}
