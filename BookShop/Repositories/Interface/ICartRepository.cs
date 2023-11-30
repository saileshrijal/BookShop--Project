using System.Linq.Expressions;
using BookShop.Models;

namespace BookShop.Repositories.Interface;

public interface ICartRepository : IRepository<Cart>
{
    Task<List<Cart>> FindByWithBooks(Expression<Func<Cart, bool>> expression);
}