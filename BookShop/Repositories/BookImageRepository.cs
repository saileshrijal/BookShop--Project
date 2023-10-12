using System.Linq.Expressions;
using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;

namespace BookShop.Repositories;

public class BookImageRepository : Repository<BookImage>, IBookImageRepository
{
    public BookImageRepository(ApplicationDbContext context) : base(context)
    {
    }
}