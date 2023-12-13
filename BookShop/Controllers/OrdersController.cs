using BookShop.Constants;
using BookShop.Models;
using BookShop.Repositories.Interface;
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
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrdersController(IOrderRepository orderRepository, 
        UserManager<ApplicationUser> userManager)
    {
        _orderRepository = orderRepository;
        _userManager = userManager;
    }
    
    public async Task<IActionResult> Index()
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var orders = await _orderRepository
            .FindByWithOrderDetailsAsync(x=>loggedInUser != null && x.ApplicationUserId == loggedInUser.Id);
        var vm = orders.Select(x => new UserOrderIndexVm()
        {
            Id = x.Id,
            ApplicationUserId = x.ApplicationUserId,
            DateOfOrder = x.DateOfOrder,
            OrderTotal = x.OrderTotal,
            OrderDetails = x.OrderDetails.Select(od => new OrderDetailsIndexVm()
            {
                Id = od.Id,
                BookId = od.BookId,
                Book = new BookDetailsVm()
                {
                    Id = od.Book?.Id ?? 0,
                    Name = od.Book?.Name,
                    Description = od.Book?.Description,
                    Price = od.Book?.Price ?? 0,
                    BookImages = od.Book?.BookImages?.Select(bi => new BookImageVm()
                    {
                        FileName = bi.Path
                    }).ToList()
                },
                Price = od.Price,
                Quantity = od.Quantity,
                OrderId = od.OrderId,
                OrderStatus = od.OrderStatus,
                PaymentStatus = od.PaymentStatus,
                DateOfPayment = od.DateOfPayment,
                DateOfOrderDelivered = od.DateOfOrderDelivered,
                DateOfOrderApproved = od.DateOfOrderApproved,
                DateOfOrderShipped = od.DateOfOrderShipped,
                DateOfOrderCancelled = od.DateOfOrderCancelled,
            }).ToList()
        }).ToList();
        return View(vm);
    }
}