using System.Linq.Expressions;
using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Order>> FindByWithOrderDetailsAsync(Expression<Func<Order, bool>> predicate)
    {
        return await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Book)
            .ThenInclude(x=>x.BookImages)
            .Where(predicate)
            .ToListAsync();
    }
}