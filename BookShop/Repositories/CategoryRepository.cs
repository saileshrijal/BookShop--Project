using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;

namespace BookShop.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}