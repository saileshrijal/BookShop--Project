using BookShop.Dtos.BlogDto;

namespace BookShop.Services.Interface;

public interface IBlogService
{
    Task AddAsync(AddBlogDto addBlogDto);
    Task EditAsync(EditBlogDto editBlogDto);
    Task DeleteAsync(int id);
    Task ToggleStatusAsync(int id);
}