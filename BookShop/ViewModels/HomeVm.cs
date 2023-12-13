using BookShop.ViewModels.BlogVm;
using BookShop.ViewModels.BookVm;

namespace BookShop.ViewModels;

public class HomeVm
{
    public List<BookIndexVm>? Books { get; set; }
    public List<BookIndexVm>? RecentBooks { get; set; }
    public List<BookIndexVm>? BestSellerBooks { get; set; }
    public List<BookIndexVm>? CategoryWiseBooks { get; set; }
    public List<BlogIndexVm>? Blogs { get; set; }
}