using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Constants;
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
using Stripe.Checkout;

namespace BookShop.Controllers;

[Authorize(Roles = UserRoles.Customer)]
public class CheckoutController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICartRepository _cartRepository;
    private readonly INotyfService _notyfService;
    private readonly IUserAddressRepository _userAddressRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserAddressService _userAddressService;
    private readonly IOrderService _orderService;
    private readonly IOrderRepository _orderRepository;
    private readonly IBookService _bookService;
    
    public CheckoutController(
        ICartService cartService, 
        ICartRepository cartRepository, 
        INotyfService notyfService,
        IUserAddressRepository userAddressRepository,
        UserManager<ApplicationUser> userManager,
        IUserAddressService userAddressService,
        IOrderService orderService,
        IOrderRepository orderRepository,
        IBookService bookService)
    {
        _cartService = cartService;
        _cartRepository = cartRepository;
        _notyfService = notyfService;
        _userAddressRepository = userAddressRepository;
        _userManager = userManager;
        _userAddressService = userAddressService;
        _orderService = orderService;
        _orderRepository = orderRepository;
        _bookService = bookService;
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
        var orderId = await _orderService.AddAndReturnIdAsync(orderDto);
        foreach (var book in orderDto.OrderDetails)
        {
            await _bookService.SubtractQuantityAsync(book.BookId, book.Quantity);
        }

        var domain = "http://localhost:5197";
        var options = new SessionCreateOptions()
        {
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
            SuccessUrl = $"{domain}/checkout/success/{orderId}",
            CancelUrl = $"{domain}/checkout",
        };

        foreach (var cartItem in carItems)
        {
            var lineItems = new SessionLineItemOptions()
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmount = (long)cartItem.Book?.Price * 100,
                    Currency = "npr",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = cartItem.Book.Name
                    }
                },
                Quantity = cartItem.Quantity
            };
            options.LineItems.Add(lineItems);
        }
        
        var service = new SessionService();
        var session = await service.CreateAsync(options);
        
        await _orderService.PayAsync(orderId, session.Id, session.PaymentIntentId);
        
        await _cartService.ClearCartAsync(loggedInUser.Id);
        _notyfService.Success("Order has been placed successfully");
        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }
    
    [HttpGet]
    public async Task<IActionResult> Success(int id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
        var service = new SessionService();
        var session = service.Get(order.SessionId);
        if (session.PaymentStatus.ToLower() == "paid")
        {
            await _orderService.UpdateOrderStatusAsync(order.Id, OrderStatus.Approved, PaymentStatus.Approved);
        }
        return View(id);
    }
}