using BookShop.Dtos.CategoryDto;
using BookShop.Helpers.Interface;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStringHelper _stringHelper;

    public CategoryService(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IStringHelper stringHelper)
    {
        _unitOfWork = unitOfWork;
        _categoryRepository = categoryRepository;
        _stringHelper = stringHelper;
    }

    public async Task AddAsync(AddCategoryDto addCategoryDto)
    {
        var category = new Category()
        {
            Name = addCategoryDto.Name,
            Description = addCategoryDto.Description,
            Slug = _stringHelper.GenerateSlug(addCategoryDto.Name)
        };
        await _unitOfWork.AddAsync(category);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id) ?? throw new Exception("Category not found");
        await _unitOfWork.DeleteAsync(category);
        await _unitOfWork.SaveAsync();
    }

    public async Task EditAsync(EditCategoryDto editUnitDto)
    {
        var category = await _categoryRepository.GetByIdAsync(editUnitDto.Id) ?? throw new Exception("Category not found");
        category.Name = editUnitDto.Name;
        category.Description = editUnitDto.Description;
        await _unitOfWork.SaveAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id) ?? throw new Exception("Category not found");
        category.Status = !category.Status;
        await _unitOfWork.SaveAsync();
    }
}