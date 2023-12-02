using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Dtos.OrderDetailsDto;
using BookShop.Dtos.OrderDto;
using BookShop.Dtos.UserAddressDto;
using BookShop.Enum;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.BookVm;
using BookShop.ViewModels.CartVm;
using BookShop.ViewModels.CheckoutVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

[Authorize]
public class CheckoutController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICartRepository _cartRepository;
    private readonly INotyfService _notyfService;
    private readonly IUserAddressRepository _userAddressRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserAddressService _userAddressService;
    private readonly IOrderService _orderService;
    
    public CheckoutController(
        ICartService cartService, 
        ICartRepository cartRepository, 
        INotyfService notyfService,
        IUserAddressRepository userAddressRepository,
        UserManager<ApplicationUser> userManager,
        IUserAddressService userAddressService,
        IOrderService orderService)
    {
        _cartService = cartService;
        _cartRepository = cartRepository;
        _notyfService = notyfService;
        _userAddressRepository = userAddressRepository;
        _userManager = userManager;
        _userAddressService = userAddressService;
        _orderService = orderService;
    }
    
    public async Task<IActionResult> Index()
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var cartItems = await _cartRepository.FindByWithBooks(x=>x.ApplicationUserId == loggedInUser.Id);
        var carItems = cartItems?.Select(x=> new CartItemVm()
        {
            Id = x.Id,
            BookId = x.BookId,
            Book = new BookDetailsVm
            {
                Id = x.Book.Id,
                Name = x.Book.Name,
                CategoryNames = x.Book.BookCategories?.Select(x => x.Category?.Name).ToList(),
                FeaturedImage = x.Book.FeaturedImagePath,
                Price = x.Book.Price,
                Description = x.Book.Description,
                ShortDescription = x.Book.ShortDescription,
                CreatedDate = x.Book.CreatedDate,
                Slug = x.Book.Slug,
                Status = x.Book.Status,
                BestSeller = x.Book.BestSeller,
                BookImages = x.Book.BookImages?.Select(x => new BookImageVm
                {
                    FileName = x.Path,
                    Alt = x.Alt,
                    DisplayOrder = x.DisplayOrder
                }).ToList()
            },
            Quantity = x.Count,
            Amount = x.Book.Price * x.Count
        }).ToList();
        var cart = new CartIndexVm
        {
            CartItems = carItems,
            TotalAmount = carItems?.Sum(x => x.Amount) ?? 0
        };
        var userAddress = await _userAddressRepository
            .GetByAsync(x => x.ApplicationUserId == loggedInUser!.Id && x.AddressType == AddressType.Billing)
            .ConfigureAwait(false);
        var vm = new CheckoutIndexVm()
        {
            Cart = cart,
            FirstName = loggedInUser?.FirstName,
            LastName = loggedInUser?.LastName,
            Address = userAddress?.StreetAddress,
            City = userAddress?.City,
            PhoneNumber = userAddress?.PhoneNumber,
            State = userAddress?.State,
            PostalCode = userAddress?.PostalCode,
            Country = userAddress?.Country,
            Email = loggedInUser?.Email,
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Index(CheckoutIndexVm vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var loggedInUser = await _userManager.GetUserAsync(User);
        var cartItems = await _cartRepository.FindByWithBooks(x=>x.ApplicationUserId == loggedInUser.Id);
        var carItems = cartItems?.Select(x=> new CartItemVm()
        {
            Id = x.Id,
            BookId = x.BookId,
            Book = new BookDetailsVm
            {
                Id = x.Book.Id,
                Name = x.Book.Name,
                CategoryNames = x.Book.BookCategories?.Select(x => x.Category?.Name).ToList(),
                FeaturedImage = x.Book.FeaturedImagePath,
                Price = x.Book.Price,
                Description = x.Book.Description,
                ShortDescription = x.Book.ShortDescription,
                CreatedDate = x.Book.CreatedDate,
                Slug = x.Book.Slug,
                Status = x.Book.Status,
                BestSeller = x.Book.BestSeller,
                BookImages = x.Book.BookImages?.Select(x => new BookImageVm
                {
                    FileName = x.Path,
                    Alt = x.Alt,
                    DisplayOrder = x.DisplayOrder
                }).ToList()
            },
            Quantity = x.Count,
            Amount = x.Book.Price * x.Count
        }).ToList();
        var cart = new CartIndexVm
        {
            CartItems = carItems,
            TotalAmount = carItems?.Sum(x => x.Amount) ?? 0
        };
        var userAddress = await _userAddressRepository
            .GetByAsync(x => x.ApplicationUserId == loggedInUser!.Id && x.AddressType == AddressType.Billing)
            .ConfigureAwait(false);
        if (userAddress == null)
        {
            var dto = new AddUserAddressDto()
            {
                ApplicationUserId = loggedInUser.Id,
                AddressType = AddressType.Billing,
                City = vm.City,
                Country = vm.Country,
                PhoneNumber = vm.PhoneNumber,
                PostalCode = vm.PostalCode,
                State = vm.State,
                StreetAddress = vm.Address
            };
            await _userAddressService.AddAsync(dto);
        }
        
        var orderDto = new AddOrderDto()
        {
            ApplicationUserId = loggedInUser.Id,
            DateOfOrder = DateTime.Now,
            OrderTotal = cart.TotalAmount,
            OrderStatus = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            OrderDetails = cartItems?.Select(x => new AddOrderDetailsDto()
            {
                BookId = x.BookId,
                Price = x.Book.Price,
                Quantity = x.Count
            }).ToList()
        };
        await _orderService.AddAsync(orderDto);
        await _cartService.ClearCartAsync(loggedInUser.Id);
        _notyfService.Success("Order has been placed successfully");
        return RedirectToAction(nameof(Success));
    }
    
    [HttpGet]
    public IActionResult Success()
    {
        return View();
    }
}