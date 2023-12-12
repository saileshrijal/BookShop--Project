namespace BookShop.ViewModels.UserVm;

public class UserIndexVm
{
    public string? Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public List<string>? Roles { get; set; }
    public bool Status { get; set; }
    public DateTime? CreatedDate { get; set; }
}