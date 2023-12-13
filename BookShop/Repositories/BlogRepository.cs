using System.Linq.Expressions;
using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Repositories;

public class BlogRepository : Repository<Blog>, IBlogRepository
{
    public BlogRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<Blog>> GetAllWithUserAsync()
    {
        return await _context.Blogs.Include(x => x.ApplicationUser).ToListAsync();
    }

    public async Task<List<Blog>> GetAllWithUserByAsync(Expression<Func<Blog, bool>> expression)
    {
        return await _context.Blogs.Include(x => x.ApplicationUser).Where(expression).ToListAsync();
    }

    public async Task<Blog> GetWithUserByIdAsync(int id)
    {
        return await _context.Blogs.Include(x => x.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id);
    }
}