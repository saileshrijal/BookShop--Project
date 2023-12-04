using System.Linq.Expressions;
using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repositories;

public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
{
    public WishlistRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Wishlist>> GetWithBooksAsync(Expression<Func<Wishlist, bool>> expression)
    {
        return await _context.Wishlists
            .Where(expression)
            .Include(x => x.Book)
            .ThenInclude(b=>b.BookImages)
            .ToListAsync();
    }
}