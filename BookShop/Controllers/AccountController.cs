using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Constants;
using BookShop.Models;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

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

    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm, string? returnUrl = null)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var user = await _userManager.FindByNameAsync(vm.Identity)
                       ?? await _userManager.FindByEmailAsync(vm.Identity);
            if (user == null)
            {
                _notyfService.Error("User not found");
                return View(vm);
            }

            if (!await _userManager.IsInRoleAsync(user, UserRoles.Customer))
            {
                _notyfService.Error("User not found");
                return View(vm);
            }

            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, false);
            if (!result.Succeeded)
            {
                _notyfService.Error("Login failed");
                return View(vm);
            }

            _notyfService.Success("Login successfully");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return View(vm);
        }
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var checkDuplicateUserName = await _userManager.FindByNameAsync(vm.Username);
            if (checkDuplicateUserName != null)
            {
                _notyfService.Error("Username already taken");
                return View();
            }

            var checkDuplicateEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (checkDuplicateEmail != null)
            {
                _notyfService.Error("Email already taken");
                return View();
            }

            var user = new ApplicationUser
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                UserName = vm.Username,
                Email = vm.Email
            };
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (!result.Succeeded)
            {
                _notyfService.Error("Register failed");
                return View(vm);
            }

            ;
            await _userManager.AddToRoleAsync(user, UserRoles.Customer);
            tx.Complete();
            _notyfService.Success("Register successfully");
            return RedirectToAction("Login");
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
        try
        {
            await _signInManager.SignOutAsync();
            _notyfService.Success("You are successfully logged out!!");
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
}