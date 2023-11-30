namespace BookShop.ViewModels.CartVm;

public class CartIndexVm
{
    public List<CartItemVm>? CartItems { get; set; }
    public decimal TotalAmount { get; set; }
}