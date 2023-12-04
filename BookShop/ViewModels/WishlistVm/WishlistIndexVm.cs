using BookShop.ViewModels.BookVm;

namespace BookShop.ViewModels.WishlistVm;

public class WishlistIndexVm
{
    public int Id { get; set; }
    public BookIndexVm? Book { get; set; }
}