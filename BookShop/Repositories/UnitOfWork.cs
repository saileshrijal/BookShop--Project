using BookShop.Data;
using BookShop.Repositories.Interface;

namespace BookShop.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync<T>(T entity) where T : class
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task AddRangeAsync<T>(List<T> entities) where T : class
    {
        await _context.Set<T>().AddRangeAsync(entities);
    }

    public async Task DeleteAsync<T>(T entity) where T : class
    {
        await Task.Run(() => _context.Set<T>().Remove(entity));
    }

    public async Task DeleteRangeAsync<T>(List<T> entities) where T : class
    {
        await Task.Run(() => _context.Set<T>().RemoveRange(entities));
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync<T>(T entity) where T : class
    {
        await Task.Run(() => _context.Set<T>().Update(entity));
    }

    public async Task UpdateRangeAsync<T>(List<T> entities) where T : class
    {
        await Task.Run(() => _context.Set<T>().UpdateRange(entities));
    }
}