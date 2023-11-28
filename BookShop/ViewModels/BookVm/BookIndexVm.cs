namespace BookShop.ViewModels.BookVm;

public class BookIndexVm
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
    public DateTime CreatedDate { get; set; }
    public List<BookImageVm>? BookImages { get; set; }
}