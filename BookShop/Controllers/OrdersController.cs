using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Constants;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.BookVm;
using BookShop.ViewModels.OrderDetailsVm;
using BookShop.ViewModels.OrderVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

[Authorize(Roles = UserRoles.Customer)]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotyfService _notyfService;

    public OrdersController(IOrderRepository orderRepository, 
        IOrderService orderService,
        UserManager<ApplicationUser> userManager,
        INotyfService notyfService)
    {
        _orderRepository = orderRepository;
        _orderService = orderService;
        _userManager = userManager;
        _notyfService = notyfService;
    }
    
    public async Task<IActionResult> Index()
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var orders = await _orderRepository
            .FindByWithOrderDetailsAsync(x=>x.ApplicationUserId == loggedInUser.Id);
        var vm = orders.Select(x => new UserOrderIndexVm()
        {
            Id = x.Id,
            ApplicationUserId = x.ApplicationUserId,
            DateOfOrder = x.DateOfOrder,
            DateOfShipping = x.DateOfShipping,
            OrderTotal = x.OrderTotal,
            OrderStatus = x.OrderStatus,
            PaymentStatus = x.PaymentStatus,
            DateOfPayment = x.DateOfPayment,
            OrderDetails = x.OrderDetails.Select(od => new OrderDetailsIndexVm()
            {
                Id = od.Id,
                BookId = od.BookId,
                Book = new BookDetailsVm()
                {
                    Id = od.Book.Id,
                    Name = od.Book.Name,
                    Description = od.Book.Description,
                    Price = od.Book.Price,
                    FeaturedImage = od.Book.FeaturedImagePath,
                    BookImages = od.Book.BookImages.Select(bi => new BookImageVm()
                    {
                        FileName = bi.Path
                    }).ToList(),
                },
                Price = od.Price,
                Quantity = od.Quantity,
                OrderId = od.OrderId,
                OrderStatus = od.OrderStatus,
                PaymentStatus = od.PaymentStatus,
                DateOfPayment = od.DateOfPayment,
                DateOfDelivered = od.DateOfDelivered
            }).ToList()
        }).ToList();
        return View(vm);
    }
}