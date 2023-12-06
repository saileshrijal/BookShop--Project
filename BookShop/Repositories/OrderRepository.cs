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

    public async Task<List<Order>> GetAllWithOrderDetailsAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Book)
            .ThenInclude(x=>x.BookImages)
            .Include(x=>x.ApplicationUser)
            .ThenInclude(x=>x.Addresses)
            .ToListAsync();
    }

    public async Task<Order> GetWithOrderDetailsAsync(Expression<Func<Order, bool>> predicate)
    {
        return await _context.Orders
            .Include(o => o.OrderDetails)
            .ThenInclude(od => od.Book)
            .ThenInclude(x=>x.BookImages)
            .Include(x=>x.ApplicationUser)
            .ThenInclude(x=>x.Addresses)
            .FirstOrDefaultAsync(predicate);
    }
}