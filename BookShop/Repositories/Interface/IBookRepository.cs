using BookShop.Models;

namespace BookShop.Repositories.Interface;

public interface IBookRepository : IRepository<Book>
{
    Task<List<Book>> GetAllWithCategoryAsync();
    Task<Book> GetWithCategoryByIdAsync(int id);
}