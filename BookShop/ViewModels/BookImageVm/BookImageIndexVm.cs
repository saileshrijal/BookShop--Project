using BookShop.PartialViewModels;
using BookShop.ViewModels.BookVm;

namespace BookShop.ViewModels.BookImageVm;

public class BookImageIndexVm
{
    public int BookId { get; set; }
    public BookInfo? Book { get; set; }
    public List<BookImageVm>? BookImages { get; set; }
}