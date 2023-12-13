namespace BookShop.Models;

public class Category : BaseModel
{
    public string? Name { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public List<BookCategory>? BookCategories { get; set; }
}