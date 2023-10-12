using BookShop.Dtos.BookImageDto;

namespace BookShop.Services.Interface;

public interface IBookImageService
{
    Task AddAsync(AddBookImageDto addBookDto);
    Task EditAsync(EditBookImageDto editBookDto);
    Task DeleteAsync(int id);
}