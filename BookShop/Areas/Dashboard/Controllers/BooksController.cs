using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Dtos.BookDto;
using BookShop.Helpers.Interface;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.BookVm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class BooksController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookService _bookService;
    private readonly ICategoryRepository _categoryRepository;
    private readonly INotyfService _notyfService;
    private readonly IFileHelper _fileHelper;

    public BooksController(IBookRepository bookRepository,
        IBookService bookService,
        INotyfService notyfService,
        ICategoryRepository categoryRepository,
        IFileHelper fileHelper)
    {
        _bookRepository = bookRepository;
        _bookService = bookService;
        _notyfService = notyfService;
        _categoryRepository = categoryRepository;
        _fileHelper = fileHelper;
    }
    
    public async Task<IActionResult> Index()
    {
        try
        {
            var books = await _bookRepository.GetAllWithCategoryAsync();
            var vm = books.Select(x=>new BookIndexVm()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                CategoryNames = x.BookCategories?.Select(x=>x.Category?.Name).ToList(),
                Status = x.Status,
                CreatedDate = x.CreatedDate,
            }).ToList();
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View();
        }
        
    }

    public async Task<IActionResult> Add()
    {
        try
        {
            var categories = await _categoryRepository
                .GetAllAsync();
            var vm = new AddBookVm()
            {
                CategoriesSelectList = categories.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddBookVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new AddBookDto()
            {
                Name = vm.Name,
                ShortDescription = vm.ShortDescription,
                Description = vm.Description,
                Price = vm.Price,
                Quantity = vm.Quantity,
                CategoryIds = vm.CategoryIds
            };
            await _bookService.AddAsync(dto);
            _notyfService.Success("Book added successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View(vm);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var book = await _bookRepository.GetWithCategoryByIdAsync(id) 
                ?? throw new Exception("Book not found");
            var categories = await _categoryRepository
                .GetAllAsync();
            var vm = new EditBookVm()
            {
                Description = book.Description,
                Id = book.Id,
                Name = book.Name,
                Price = book.Price,
                ShortDescription = book.ShortDescription,
                Quantity = book.Quantity,
                CategoryIds = book.BookCategories?.Select(x => x.CategoryId).ToList(),
                CategoriesSelectList = categories.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList()
            };
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(EditBookVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new EditBookDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                ShortDescription = vm.ShortDescription,
                Description = vm.Description,
                Price = vm.Price,
                CategoryIds = vm.CategoryIds
            };
            await _bookService.EditAsync(dto);
            _notyfService.Success("Book edited successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return View(vm);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var book = await _bookRepository.GetByIdAsync(id);
            await _bookService.DeleteAsync(id);
            _notyfService.Success("Book deleted successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            await _bookService.ToggleStatusAsync(id);
            _notyfService.Success("Book status changed successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}