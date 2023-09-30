namespace BookShop.Repositories.Interface;

public interface IUnitOfWork
{
    Task AddAsync<T>(T entity) where T : class;
    Task AddRangeAsync<T>(List<T> entities) where T : class;
    Task UpdateAsync<T>(T entity) where T : class;
    Task UpdateRangeAsync<T>(List<T> entities) where T : class;
    Task DeleteAsync<T>(T entity) where T : class;
    Task DeleteRangeAsync<T>(List<T> entities) where T : class;
    Task SaveAsync();
}