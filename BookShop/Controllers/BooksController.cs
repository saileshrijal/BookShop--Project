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

public class BooksController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICartService _cartService;
    private readonly INotyfService _notyfService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IWishlistRepository _wishlistRepository;

    public BooksController(
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
        var vm = books.Select(x => new BookIndexVm
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
        }).ToList();
        return View(vm);
    }

    [HttpGet("/book/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var book = await _bookRepository.GetWithCategoryAndImagesAsync(slug);
        if (book == null) return NotFound();
        var vm = new BookDetailsVm
        {
            Id = book.Id,
            Name = book.Name,
            CategoryNames = book.BookCategories?.Select(x => x.Category?.Name).ToList(),
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
    
    [HttpGet("/category/{slug}")]
    public async Task<IActionResult> Category(string slug)
    {
        var checkCategory = await _categoryRepository.GetByAsync(x=>x.Slug == slug);
        if (checkCategory == null) return NotFound();
        ViewBag.CategoryName = checkCategory.Name;
        var books = await _bookRepository.GetAllByCategorySlugWithCategoryAndImagesAsync(slug);
        var vm = books.Select(x => new BookIndexVm
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
        if(qty > book.Quantity)
        {
            _notyfService.Error("Not enough stock!");
            return RedirectToAction(nameof(Details), "Books", new {slug = book.Slug});
        }
        var cart = new AddCartDto()
        {
            BookId = book.Id,
            Quantity = qty,
            ApplicationUserId = loggedInUser?.Id
        };
        await _cartService.AddAsync(cart);
        _notyfService.Success("Add to cart successfully!");
        return RedirectToAction(nameof(Details), "Books", new {slug = book.Slug});
    }
}