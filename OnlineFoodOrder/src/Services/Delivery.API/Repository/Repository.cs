using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Delivery.API.Data;
using FoodOrder.Shared.Repository;

namespace Delivery.API.Repository; 

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly DeliveryDbContext _db;
    private readonly DbSet<T> _set;

    public Repository(DeliveryDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id) =>
        await _set.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() =>
        await _set.ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
        await _set.Where(predicate).ToListAsync();

    public async Task AddAsync(T entity) =>
        await _set.AddAsync(entity);

    public void Update(T entity) =>
        _set.Update(entity);

    public void Remove(T entity) =>
        _set.Remove(entity);
}