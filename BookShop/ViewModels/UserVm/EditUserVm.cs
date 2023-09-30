using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.ViewModels.UserVm;

public class EditUserVm
{
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public List<string>? Roles { get; set; }
    public List<SelectListItem>? RolesSelectListItems { get; set; }
}