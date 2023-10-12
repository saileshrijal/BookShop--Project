using BookShop.Dtos.BookDto;

namespace BookShop.Services.Interface;

public interface IBookService
{
    Task AddAsync(AddBookDto addBookDto);
    Task EditAsync(EditBookDto editBookDto);
    Task DeleteAsync(int id);
    Task ToggleStatusAsync(int id);
}