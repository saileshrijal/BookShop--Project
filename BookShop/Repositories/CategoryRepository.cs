using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Category>> GetWithBooks()
    {
        return await _context.Categories
            .Include(x => x.BookCategories)
            .ThenInclude(x => x.Book)
            .ToListAsync();
    }
}