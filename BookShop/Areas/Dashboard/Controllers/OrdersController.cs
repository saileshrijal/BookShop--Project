using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Constants;
using BookShop.Enum;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.BookVm;
using BookShop.ViewModels.OrderDetailsVm;
using BookShop.ViewModels.OrderVm;
using BookShop.ViewModels.UserVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Dashboard.Controllers;

[Authorize(Roles = UserRoles.Admin)]
[Area("Dashboard")]
public class OrdersController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailsRepository _orderDetailsRepository;
    private readonly IOrderService  _orderService;
    private readonly INotyfService _notyfService;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrdersController(
        IOrderRepository orderRepository,
        IOrderDetailsRepository orderDetailsRepository,
        IOrderService orderService,
        INotyfService notyfService,
        UserManager<ApplicationUser> userManager)
    {
        _orderRepository = orderRepository;
        _orderDetailsRepository = orderDetailsRepository;
        _orderService = orderService;
        _notyfService = notyfService;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var orders = await _orderRepository.GetAllWithOrderDetailsAsync();
            var vm = orders.Select(x=> new OrderIndexVm()
            {
                Id = x.Id,
                DateOfOrder = x.DateOfOrder,
                OrderTotal = x.OrderTotal,
                OrderDetails = x.OrderDetails.Select(od=> new OrderDetailsIndexVm()
                {
                    Id = od.Id,
                    Book = new BookDetailsVm()
                    {
                        Id = od.Book.Id,
                        Name = od.Book.Name,
                        Price = od.Book.Price,
                        BookImages = od.Book.BookImages.Select(x=> new BookImageVm()
                        {
                            FileName = x.Path
                        }).ToList()
                    },
                    Quantity = od.Quantity,
                    Price = od.Price,
                }).ToList(),
                User = new UserIndexVm()
                {
                    Id = x.ApplicationUser.Id,
                    FullName = x.ApplicationUser.FullName,
                    Email = x.ApplicationUser.Email,
                    PhoneNumber = x.ApplicationUser.PhoneNumber,
                    CreatedDate = x.ApplicationUser.CreatedDate,
                    Status = x.ApplicationUser.Status
                },
                UserAddress = new UserAddressVm()
                {
                    Id = x.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).Id,
                    StreetAddress = x.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).StreetAddress,
                    City = x.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).City,
                    Country = x.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).Country,
                    State = x.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).State,
                    PhoneNumber = x.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).PhoneNumber,
                    PostalCode = x.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).PostalCode,
                }
            }).ToList();
            
            return View(vm);
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var order = await _orderRepository.GetWithOrderDetailsAsync(x=>x.Id == id);
            var vm = new OrderIndexVm()
            {
                Id = order.Id,
                DateOfOrder = order.DateOfOrder,
                OrderTotal = order.OrderTotal,
                OrderDetails = order.OrderDetails.Select(od=> new OrderDetailsIndexVm()
                {
                    Id = od.Id,
                    Book = new BookDetailsVm()
                    {
                        Id = od.Book.Id,
                        Name = od.Book.Name,
                        Price = od.Book.Price,
                        BookImages = od.Book.BookImages.Select(x=> new BookImageVm()
                        {
                            FileName = x.Path
                        }).ToList()
                    },
                    Quantity = od.Quantity,
                    Price = od.Price,
                    OrderStatus = od.OrderStatus,
                    PaymentStatus = od.PaymentStatus,
                    Total = od.Total
                }).ToList(),
                User = new UserIndexVm()
                {
                    Id = order.ApplicationUser.Id,
                    FullName = order.ApplicationUser.FullName,
                    Email = order.ApplicationUser.Email,
                    PhoneNumber = order.ApplicationUser.PhoneNumber,
                    CreatedDate = order.ApplicationUser.CreatedDate,
                    Status = order.ApplicationUser.Status
                },
                UserAddress = new UserAddressVm()
                {
                    Id = order.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).Id,
                    StreetAddress = order.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).StreetAddress,
                    City = order.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).City,
                    Country = order.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).Country,
                    State = order.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).State,
                    PhoneNumber = order.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).PhoneNumber,
                    PostalCode = order.ApplicationUser.Addresses.FirstOrDefault(x => x.AddressType == AddressType.Billing).PostalCode,
                }
            };
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
    
    public async Task<IActionResult> ChangeOrderStatus(int id, OrderStatus orderStatus)
    {
        try
        {
            var orderDetails = await _orderDetailsRepository.GetByIdAsync(id);
            if (orderDetails == null)
            {
                _notyfService.Error("Order not found");
                return RedirectToAction(nameof(Index));
            }
            await _orderService.UpdateOrderStatusAsync(id, orderStatus);
            _notyfService.Success("Order status updated successfully");
            return RedirectToAction(nameof(Details), new {id = orderDetails.OrderId});
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}