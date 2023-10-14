namespace BookShop.Dtos.BookDto;

public class EditBookDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? FeaturedImage { get; set; }
    public List<int>? CategoryIds { get; set; }
}