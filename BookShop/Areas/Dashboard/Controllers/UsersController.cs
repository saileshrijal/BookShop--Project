using System.Transactions;
using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Constants;
using BookShop.Models;
using BookShop.ViewModels.UserVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Dashboard.Controllers;

[Authorize]
[Area("Dashboard")]
public class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly INotyfService _notyfService;

    public UsersController(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        INotyfService notyfService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _notyfService = notyfService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _userManager
            .Users
            .Where(x => x.Id != _userManager.GetUserId(User))
            .ToListAsync();
        //users with no role of customer
        users = users.Where(x => _userManager.GetRolesAsync(x).Result.All(y => y != UserRoles.Customer)).ToList();        
        
        var vm = users.Select(x => new UserIndexVm
        {
            Id = x.Id,
            FullName = x.FullName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            ProfilePictureUrl = x.ProfilePictureUrl,
            Roles = _userManager.GetRolesAsync(x).Result.ToList(),
            Status = x.Status,
            CreatedDate = x.CreatedDate
        }).ToList();
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        var vm = new AddUserVm()
        {
            RolesSelectListItems = roles.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Name
            }).ToList()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddUserVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var user = new ApplicationUser
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                UserName = vm.Username,
                PhoneNumber = vm.PhoneNumber,
                RegistrationDate = DateTime.UtcNow,
            };
            var result = await _userManager.CreateAsync(user, vm.Password!);
            if (result.Succeeded)
            {
                var roles = vm.Roles ?? new();
                foreach (var role in roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                transactionScope.Complete();
                _notyfService.Success("User added successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error(result.Errors.FirstOrDefault()?.Description ?? "Something went wrong");
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View(vm);
        }
    }

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                _notyfService.Error("User not found");
                return RedirectToAction(nameof(Index));
            }
            user.Status = !user.Status;
            await _userManager.UpdateAsync(user);
            _notyfService.Success("User status updated successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            _notyfService.Error("User not found");
            return RedirectToAction(nameof(Index));
        }
        if(await _userManager.IsInRoleAsync(user, UserRoles.Customer))
        {
            _notyfService.Error("You can't edit customer");
            return RedirectToAction(nameof(Index));
        }
        var roles = await _roleManager.Roles.ToListAsync();
        var vm = new EditUserVm()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Roles = _userManager.GetRolesAsync(user).Result.ToList(),
            RolesSelectListItems = roles.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Name
            }).ToList()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUserVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.FindByIdAsync(vm.Id!);
            if (user is null)
            {
                _notyfService.Error("User not found");
                return RedirectToAction(nameof(Index));
            }
            if (await _userManager.IsInRoleAsync(user, UserRoles.Customer))
            {
                _notyfService.Error("You can't edit customer");
                return RedirectToAction(nameof(Index));
            }
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.PhoneNumber = vm.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var roles = vm.Roles ?? new();
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
                foreach (var role in roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
                transactionScope.Complete();
                _notyfService.Success("User updated successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error(result.Errors.FirstOrDefault()?.Description ?? "Something went wrong");
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View(vm);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                _notyfService.Error("User not found");
                return RedirectToAction(nameof(Index));
            }
            if (await _userManager.IsInRoleAsync(user, UserRoles.Customer))
            {
                _notyfService.Error("You can't delete customer");
                return RedirectToAction(nameof(Index));
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _notyfService.Success("User deleted successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error(result.Errors.FirstOrDefault()?.Description ?? "Something went wrong");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> ResetPassword(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null)
        {
            _notyfService.Error("User not found");
            return RedirectToAction(nameof(Index));
        }
        if (await _userManager.IsInRoleAsync(user, UserRoles.Customer))
        {
            _notyfService.Error("You can't delete customer");
            return RedirectToAction(nameof(Index));
        }
        var vm = new ResetPasswordVm()
        {
            Id = user.Id,
            Email = user.Email
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.FindByIdAsync(vm.Id!);
            if (user is null)
            {
                _notyfService.Error("User not found");
                return RedirectToAction(nameof(Index));
            }
            if (await _userManager.IsInRoleAsync(user, UserRoles.Customer))
            {
                _notyfService.Error("You can't delete customer");
                return RedirectToAction(nameof(Index));
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, vm.Password!);
            if (result.Succeeded)
            {
                _notyfService.Success("Password reset successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error(result.Errors.FirstOrDefault()?.Description ?? "Something went wrong");
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View(vm);
        }
    }

    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordVm vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                _notyfService.Error("Logged in user not found");
                return RedirectToAction(nameof(Index));
            }
            if (await _userManager.IsInRoleAsync(user, UserRoles.Customer))
            {
                _notyfService.Error("You can't delete customer");
                return RedirectToAction(nameof(Index));
            }
            var result = await _userManager.ChangePasswordAsync(user, vm.OldPassword!, vm.NewPassword!);
            if (result.Succeeded)
            {
                _notyfService.Success("Password changed successfully");
                return RedirectToAction(nameof(Index));
            }
            _notyfService.Error(result.Errors.FirstOrDefault()?.Description ?? "Something went wrong");
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View(vm);
        }
    }
}