using Microsoft.AspNetCore.Identity;

namespace BookShop.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool Status { get; set; } = true;
    public string? ProfilePictureUrl { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string FullName => $"{FirstName} {LastName}";
}