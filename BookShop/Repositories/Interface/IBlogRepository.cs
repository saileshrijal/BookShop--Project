using System.Linq.Expressions;
using BookShop.Models;

namespace BookShop.Repositories.Interface;

public interface IBlogRepository : IRepository<Blog>
{
    Task<List<Blog>> GetAllWithUserAsync();
    Task<List<Blog>> GetAllWithUserByAsync(Expression<Func<Blog, bool>> expression);
    Task<Blog> GetWithUserByIdAsync(int id);
    Task<Blog> GetWithUserByAsync(Expression<Func<Blog, bool>> expression);
}