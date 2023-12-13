using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Constants;
using BookShop.Models;
using BookShop.Repositories;
using BookShop.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

[Authorize(Roles = $"{UserRoles.User},{UserRoles.Admin}")]
public class DashboardController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitRepository _unitRepository;
    private readonly IBookRepository _bookRepository;
    private readonly INotyfService _notyfService;

    public DashboardController(UserManager<ApplicationUser> userManager, 
        ICategoryRepository categoryRepository, 
        IUnitRepository unitRepository, 
        IBookRepository bookRepository,
        INotyfService notyfService)
    {
        _userManager = userManager;
        _categoryRepository = categoryRepository;
        _unitRepository = unitRepository;
        _bookRepository = bookRepository;
        _notyfService = notyfService;
    }
    
    public async Task<IActionResult> Index()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View();
        }
    }
}