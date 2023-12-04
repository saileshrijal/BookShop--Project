using System.Linq.Expressions;
using BookShop.Models;

namespace BookShop.Repositories.Interface;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> FindByWithOrderDetailsAsync(Expression<Func<Order, bool>> predicate);
}