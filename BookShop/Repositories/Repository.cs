using System.Linq.Expressions;
using BookShop.Data;
using BookShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public Task<List<T>> GetAllAsync()
    {
        return _dbSet.ToListAsync();
    }

    public async Task<T> GetByAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate) ?? default!;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id) ?? default!;
    }
}