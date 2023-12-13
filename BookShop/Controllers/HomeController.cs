using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels;
using BookShop.ViewModels.BookVm;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

public class HomeController : Controller
{
    private readonly IBookRepository _bookRepository;

    public HomeController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
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
        return View(vm);
    }
}