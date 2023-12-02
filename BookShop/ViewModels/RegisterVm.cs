using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels;

public class RegisterVm
{
    [Required(ErrorMessage = "Please enter first name")]
    public string? FirstName { get; set; }
    
    [Required(ErrorMessage = "Please enter last name")]
    public string? LastName { get; set; }
    
    [Required(ErrorMessage = "Please enter username")]
    public string? Username { get; set; }
    
    [Required(ErrorMessage = "Please email address")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Please enter password")]
    public string? Password { get; set; }
}