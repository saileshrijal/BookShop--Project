using BookShop.Dtos.CategoryDto;

namespace BookShop.Services.Interface;

public interface ICategoryService
{
    Task AddAsync(AddCategoryDto addCategoryDto);
    Task EditAsync(EditCategoryDto editCategoryDto);
    Task DeleteAsync(int id);
    Task ToggleStatusAsync(int id);
}