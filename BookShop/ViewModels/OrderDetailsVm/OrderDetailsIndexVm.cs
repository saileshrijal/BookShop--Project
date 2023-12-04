using BookShop.ViewModels.BookVm;

namespace BookShop.ViewModels.OrderDetailsVm;

public class OrderDetailsIndexVm
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public BookDetailsVm? Book { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}