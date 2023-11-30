using System.Linq.Expressions;
using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repositories;

public class CartRepository : Repository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Cart>> FindByWithBooks(Expression<Func<Cart, bool>> expression)
    {
        return await _context.Carts
            .Include(x => x.Book)
            .ThenInclude(x => x.BookImages)
            .Where(expression)
            .ToListAsync();
    }
}