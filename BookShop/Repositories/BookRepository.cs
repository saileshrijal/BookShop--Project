using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repositories;

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Book>> GetAllWithCategoryAsync()
    {
        return await _context.Books
            .Include(x => x.BookCategories)
            .ThenInclude(x=>x.Category)
            .ToListAsync();
    }

    public async Task<Book> GetWithCategoryByIdAsync(int id)
    {
        return await _context.Books
            .Include(x => x.BookCategories)
            .ThenInclude(x=>x.Category)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}