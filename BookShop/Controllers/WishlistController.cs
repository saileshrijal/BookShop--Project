using BookShop.Dtos.WishlistDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.BookImageVm;
using BookShop.ViewModels.BookVm;
using BookShop.ViewModels.WishlistVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BookImageVm = BookShop.ViewModels.BookVm.BookImageVm;

namespace BookShop.Controllers;

[Authorize]
public class WishlistController : Controller
{
    private readonly IWishlistRepository _wishlistRepository;
    private readonly IWishlistService _wishlistService;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public WishlistController(IWishlistRepository wishlistRepository, 
        IWishlistService wishlistService,
        UserManager<ApplicationUser> userManager)
    {
        _wishlistRepository = wishlistRepository;
        _wishlistService = wishlistService;
        _userManager = userManager;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var wishlists = await _wishlistRepository
            .GetWithBooksAsync(x=>x.ApplicationuserId== loggedInUser.Id);
        var vm = wishlists.Select(x => new WishlistIndexVm
        {
            Id = x.Id,
            Book = new BookIndexVm
            {
                Id = x.Book.Id,
                Name = x.Book.Name,
                FeaturedImage = x.Book.FeaturedImagePath,
                Price = x.Book.Price,
                Quantity = x.Book.Quantity,
                Slug = x.Book.Slug,
                Status = x.Book.Status,
                BestSeller = x.Book.BestSeller,
                ShortDescription = x.Book.ShortDescription,
                Description = x.Book.Description,
                CreatedDate = x.Book.CreatedDate,
                BookImages = x.Book.BookImages.Select(b => new BookImageVm
                {
                    FileName = b.Path,
                }).ToList()
            }
        }).ToList();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleWishlist(int bookId)
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var dto = new AddWishlistDto
        {
            BookId = bookId,
            ApplicationuserId = loggedInUser.Id
        };
        await _wishlistService.ToggleAsync(dto);
        //refresh to the current page
        var currentUrl = Request.Headers["Referer"].ToString();
        return currentUrl != null ? Redirect(currentUrl) : RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    public async Task<IActionResult> RemoveWishlist(int bookId)
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var dto = new AddWishlistDto
        {
            BookId = bookId,
            ApplicationuserId = loggedInUser.Id
        };
        await _wishlistService.RemoveAsync(dto);
        //refresh to the current page
        var currentUrl = Request.Headers["Referer"].ToString();
        return currentUrl != null ? Redirect(currentUrl) : RedirectToAction("Index", "Home");
    }
}