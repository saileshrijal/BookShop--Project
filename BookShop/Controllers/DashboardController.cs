using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Constants;
using BookShop.Enum;
using BookShop.Models;
using BookShop.Repositories;
using BookShop.Repositories.Interface;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Controllers;

[Authorize(Roles = $"{UserRoles.User},{UserRoles.Admin}")]
public class DashboardController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitRepository _unitRepository;
    private readonly IBookRepository _bookRepository;
    private readonly INotyfService _notyfService;
    private readonly IOrderRepository _orderRepository;

    public DashboardController(UserManager<ApplicationUser> userManager, 
        ICategoryRepository categoryRepository, 
        IUnitRepository unitRepository, 
        IBookRepository bookRepository,
        IOrderRepository orderRepository,        
        INotyfService notyfService)
    {
        _userManager = userManager;
        _categoryRepository = categoryRepository;
        _unitRepository = unitRepository;
        _bookRepository = bookRepository;
        _notyfService = notyfService;
        _orderRepository = orderRepository;
    }
    
    public async Task<IActionResult> Index()
    {
        try
        {
            var users = await _userManager.GetUsersInRoleAsync(UserRoles.User);
            var customers = await _userManager.GetUsersInRoleAsync(UserRoles.Customer);
            var vm = new DashboardVm()
            {
                TotalCategories = await _categoryRepository.CountAsync(),
                TotalBooks = await _bookRepository.CountAsync(),
                TotalBlogs = await _bookRepository.CountAsync(),
                TotalUsers = users.Count,
                TotalCustomers = customers.Count,
                TotalOrders = await _orderRepository.CountAsync(),
                TotalShipped = await _orderRepository.CountByAsync(x => x.OrderDetails.Any(y => y.OrderStatus == OrderStatus.Shipped)),
                TotalDelivered = await _orderRepository.CountByAsync(x => x.OrderDetails.Any(y => y.OrderStatus == OrderStatus.Delivered)),
                TotalCancelled = await _orderRepository.CountByAsync(x => x.OrderDetails.Any(y => y.OrderStatus == OrderStatus.Cancelled)),
            };
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View();
        }
    }
}