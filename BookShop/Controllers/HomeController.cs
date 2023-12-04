using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Dtos.CartDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels;
using BookShop.ViewModels.BookVm;
using BookShop.ViewModels.CategoryVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

public class HomeController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICartService _cartService;
    private readonly INotyfService _notyfService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWishlistRepository _wishlistRepository;

    public HomeController(
        IBookRepository bookRepository,
        ICategoryRepository categoryRepository,
        ICartService cartService,
        INotyfService notyfService,
        UserManager<ApplicationUser> userManager,
        IWishlistRepository wishlistRepository)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
        _cartService = cartService;
        _notyfService = notyfService;
        _userManager = userManager;
        _wishlistRepository = wishlistRepository;
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
                FeaturedImage = x.FeaturedImagePath,
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

    //get by slug
    [HttpGet("/book/{slug}")]
    public async Task<IActionResult> Detail(string slug)
    {
        var book = await _bookRepository.GetWithCategoryAndImagesAsync(slug);
        if (book == null) return NotFound();
        var vm = new BookDetailsVm
        {
            Id = book.Id,
            Name = book.Name,
            CategoryNames = book.BookCategories?.Select(x => x.Category?.Name).ToList(),
            FeaturedImage = book.FeaturedImagePath,
            Price = book.Price,
            Description = book.Description,
            ShortDescription = book.ShortDescription,
            CreatedDate = book.CreatedDate,
            Status = book.Status,
            Quantity = book.Quantity,
            BestSeller = book.BestSeller,
            BookImages = book.BookImages?.Select(x => new BookImageVm
            {
                FileName = x.Path,
                Alt = x.Alt,
                DisplayOrder = x.DisplayOrder
            }).ToList(),
            AddedToFav = await _wishlistRepository
                .CheckWishlistAsync(x => x.BookId == book.Id && x.ApplicationuserId == _userManager.GetUserId(User))
        };
        var categories = await _categoryRepository
            .GetWithBooks();
        vm.CategoriesWithCount = categories.Select(x => new CategoryWithCountVm
        {
            Id = x.Id,
            Name = x.Name,
            Count = x.BookCategories?.Count ?? 0
        }).ToList();
        return View(vm);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddToCart(int id, int qty)
    {
        var book = await _bookRepository.GetWithCategoryAndImagesAsync(id);
        if (book == null) return NotFound();
        var loggedInUser = await _userManager.GetUserAsync(User);
        var cart = new AddCartDto()
        {
            BookId = book.Id,
            Quantity = qty,
            ApplicationUserId = loggedInUser?.Id
        };
        await _cartService.AddAsync(cart);
        _notyfService.Success("Add to cart successfully!");
        return RedirectToAction(nameof(Detail), "Home", new {slug = book.Slug});
    }
}