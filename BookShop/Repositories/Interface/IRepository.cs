using System.Linq.Expressions;

namespace BookShop.Repositories.Interface;

public interface IRepository<T> where T : class
{
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> FindByAsync(Expression<Func<T, bool>> predicate);
    Task<int> CountAsync();
}