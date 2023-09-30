using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.ViewModels.UserVm;

public class AddUserVm
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string>? Roles { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public List<SelectListItem>? RolesSelectListItems { get; set; }
}