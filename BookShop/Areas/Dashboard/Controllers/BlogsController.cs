using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Dtos.BlogDto;
using BookShop.Dtos.CategoryDto;
using BookShop.Helpers.Interface;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.BlogVm;
using BookShop.ViewModels.CategoryVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class BlogsController : Controller
{
    private readonly IBlogRepository _blogRepository;
    private readonly IBlogService _blogService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IFileHelper _fileHelper;
    private readonly INotyfService _notyfService;


    public BlogsController(IBlogRepository blogRepository, 
        IBlogService blogService,
        UserManager<ApplicationUser> userManager, 
        IFileHelper fileHelper,
        INotyfService notyfService)
    {
        _blogRepository = blogRepository;
        _userManager = userManager;
        _fileHelper = fileHelper;
        _blogService = blogService;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var blogs = await _blogRepository.GetAllAsync();
            var vm = blogs.Select(x => new BlogIndexVm()
            {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                ThumbnailUrl = x.ThumbnailUrl,
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
    public async Task<IActionResult> Add(AddBlogVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var loggedInUser = await _userManager.GetUserAsync(User);
            var dto = new AddBlogDto()
            {
                Title = vm.Title,
                ShortDescription = vm.ShortDescription,
                Description = vm.Description,
                ApplicationUserId = loggedInUser.Id
            };
            if(vm.Thumbnail != null)
            {
                var fileName = await _fileHelper.UploadFileAsync(vm.Thumbnail, "thumbnails");
                dto.ThumbnailUrl = fileName;
            }
            await _blogService.AddAsync(dto);
            _notyfService.Success("Blog added successfully");
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
            await _blogService.DeleteAsync(id);
            _notyfService.Success("blog deleted successfully");
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
        var category = await _blogRepository.GetByIdAsync(id) 
                       ?? throw new Exception("Category not found");
        if (!string.IsNullOrWhiteSpace(category.ThumbnailUrl))
        {
            await _fileHelper.DeleteFileAsync(category.ThumbnailUrl, "thumbnails");
        }
        var vm = new EditBlogVm()
        {
            Id = category.Id,
            Title = category.Title,
            Description = category.Description,
            ShortDescription = category.ShortDescription,
            ThumbnailUrl = category.ThumbnailUrl,
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditBlogVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new EditBlogDto()
            {
                Id = vm.Id,
                Title = vm.Title,
                Description = vm.Description,
                ShortDescription = vm.ShortDescription,
                ThumbnailUrl = vm.ThumbnailUrl
            };
            if(vm.Thumbnail != null)
            {
                var fileName = await _fileHelper.UploadFileAsync(vm.Thumbnail, "thumbnails");
                dto.ThumbnailUrl = fileName;
            }
            await _blogService.EditAsync(dto);
            _notyfService.Success("Blog edited successfully");
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
            await _blogService.ToggleStatusAsync(id);
            _notyfService.Success("Blogs status changed successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}