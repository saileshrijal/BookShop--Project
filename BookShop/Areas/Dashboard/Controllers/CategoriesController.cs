using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Dtos.CategoryDto;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.CategoryVm;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class CategoriesController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICategoryService _categoryService;
    private readonly INotyfService _notyfService;
    
    public CategoriesController(ICategoryRepository categoryRepository,
        ICategoryService categoryService,
        INotyfService notyfService)
    {
        _categoryRepository = categoryRepository;
        _categoryService = categoryService;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = await _categoryRepository.GetAllAsync();
            var vm = categories.Select(x => new CategoryIndexVm()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Status = x.Status,
                CreatedDate = x.CreatedDate
            }).ToList();
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View();
        }
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddCategoryVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new AddCategoryDto()
            {
                Name = vm.Name,
                Description = vm.Description
            };
            await _categoryService.AddAsync(dto);
            _notyfService.Success("Category added successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View(vm);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _categoryService.DeleteAsync(id);
            _notyfService.Success("Category deleted successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id) 
                       ?? throw new Exception("Category not found");
        var vm = new EditCategoryVm()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditCategoryVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new EditCategoryDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description
            };
            await _categoryService.EditAsync(dto);
            _notyfService.Success("Category edited successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View(vm);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            await _categoryService.ToggleStatusAsync(id);
            _notyfService.Success("Category status changed successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}