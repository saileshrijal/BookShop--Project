using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels;

public class LoginVm
{
    [Required(ErrorMessage = "Please enter username or email address")]
    public string Identity { get; set; } = null!;
    [Required(ErrorMessage = "Please enter password")]
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}