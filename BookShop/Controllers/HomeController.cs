using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels;
using BookShop.ViewModels.BlogVm;
using BookShop.ViewModels.BookVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

public class HomeController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IBlogRepository _blogRepository;

    public HomeController(IBookRepository bookRepository,
        IBlogRepository blogRepository)
    {
        _bookRepository = bookRepository;
        _blogRepository = blogRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var books = await _bookRepository.GetAllWithCategoryAndImagesAsync();
        var vm = new HomeVm
        {
            Books = books.Select(x => new BookIndexVm
            {
                Id = x.Id,
                Name = x.Name,
                CategoryNames = x.BookCategories?.Select(x => x.Category?.Name).ToList(),
                Price = x.Price,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                CreatedDate = x.CreatedDate,
                Slug = x.Slug,
                Status = x.Status,
                BestSeller = x.BestSeller,
                BookImages = x.BookImages?.Select(x => new BookImageVm
                {
                    FileName = x.Path,
                    Alt = x.Alt,
                    DisplayOrder = x.DisplayOrder
                }).ToList()
            }).ToList()
        };
        vm.RecentBooks = vm.Books.OrderByDescending(x => x.CreatedDate).Take(5).ToList();
        vm.BestSellerBooks = vm.Books.Where(x => x.BestSeller).OrderByDescending(x => x.CreatedDate).Take(5).ToList();
        var blogs = await _blogRepository.GetAllWithUserAsync();
        vm.Blogs = blogs.Select(x => new BlogIndexVm
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            ShortDescription = x.ShortDescription,
            CreatedDate = x.CreatedDate,
            Slug = x.Slug,
            Status = x.Status,
            ThumbnailUrl = x.ThumbnailUrl,
            AuthorName = x.ApplicationUser?.FullName,
        }).ToList();
        return View(vm);
    }
}