namespace BookShop.Models;

public class Book : BaseModel
{
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? FeaturedImagePath { get; set; }
    public List<BookCategory>? BookCategories { get; set; }
}