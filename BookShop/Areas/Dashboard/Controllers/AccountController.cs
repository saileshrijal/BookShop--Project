using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Constants;
using BookShop.Models;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;

namespace BookShop.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly INotyfService _notyfService;

    public AccountController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        INotyfService notyfService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _notyfService = notyfService;
    }

    // GET
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var checkUser = await _userManager.FindByNameAsync(vm.Identity)
                            ?? await _userManager.FindByEmailAsync(vm.Identity);
            if (checkUser is null)
            {
                _notyfService.Error("Email/username not found");
                return View(vm);
            }
            if (await _userManager.IsInRoleAsync(checkUser, UserRoles.Customer))
            {
                _notyfService.Error("You are not allowed to access this area");
                return View(vm);
            }
            var checkPassword = await _userManager
                .CheckPasswordAsync(checkUser, vm.Password);
            if (!checkPassword)
            {
                _notyfService.Error("Password is incorrect");
                return View(vm);
            }
            var result = await _signInManager.PasswordSignInAsync(
                checkUser, vm.Password, vm.RememberMe, false);
            if (result.Succeeded)
            {
                _notyfService.Success("Login successfully");
                return RedirectToAction(nameof(Index), "Dashboard", new {area=""});
            }
            _notyfService.Error("Login failed");
            return View(vm);
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return View(vm);
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _notyfService.Success("Logout successfully");
        return RedirectToAction(nameof(Login));
    }
}