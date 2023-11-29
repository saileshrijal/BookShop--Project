using BookShop.ViewModels.CategoryVm;

namespace BookShop.ViewModels.BookVm;

public class BookDetailsVm
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? FeaturedImage { get; set; }
    public List<string>? CategoryNames { get; set; }
    public bool Status { get; set; }
    public bool BestSeller { get; set; }
    public string? Slug { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<BookImageVm>? BookImages { get; set; }
    public List<CategoryWithCountVm> CategoriesWithCount { get; set; }
}