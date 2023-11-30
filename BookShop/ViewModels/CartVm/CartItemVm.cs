using BookShop.ViewModels.BookVm;

namespace BookShop.ViewModels.CartVm;

public class CartItemVm
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public BookDetailsVm? Book { get; set; }
    public int Quantity { get; set; }
    public decimal Amount { get; set; }
}