using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.ViewModels;
using BookShop.ViewModels.BookVm;

namespace BookShop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBookRepository _bookRepository;

    public HomeController(
        ILogger<HomeController> logger,
        IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var books = await _bookRepository.GetAllWithCategoryAndImagesAsync();
        var vm = new HomeVm()
        {
            Books = books.Select(x => new BookIndexVm()
            {
                Id = x.Id,
                Name = x.Name,
                CategoryNames = x.BookCategories?.Select(x => x.Category?.Name).ToList(),
                FeaturedImage = x.FeaturedImagePath,
                Price = x.Price,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                CreatedDate = x.CreatedDate,
                Status = x.Status,
                BestSeller = x.BestSeller,
                BookImages = x.BookImages?.Select(x => new BookImageVm()
                {
                    FileName = x.Path,
                }).ToList()
            }).ToList()
        };
        vm.RecentBooks = vm.Books.OrderByDescending(x => x.CreatedDate).Take(5).ToList();
        vm.BestSellerBooks = vm.Books.Where(x => x.BestSeller).OrderByDescending(x => x.CreatedDate).Take(5).ToList();
        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}