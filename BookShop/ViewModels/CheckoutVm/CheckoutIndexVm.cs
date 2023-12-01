using BookShop.ViewModels.CartVm;

namespace BookShop.ViewModels.CheckoutVm;

public class CheckoutIndexVm
{
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public CartIndexVm? Cart { get; set; }
}