using BookShop.Models;

namespace BookShop.Repositories.Interface;

public interface IBookRepository : IRepository<Book>
{
    Task<List<Book>> GetAllWithCategoryAsync();
    Task<Book> GetWithCategoryByIdAsync(int id);
    Task<List<Book>> GetAllWithCategoryAndImagesAsync();
    Task<Book> GetWithCategoryAndImagesAsync(int id);
    Task<Book> GetWithCategoryAndImagesAsync(string slug);
    Task<List<Book>> GetAllByCategorySlugWithCategoryAndImagesAsync(string slug);
}