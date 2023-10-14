namespace BookShop.Models;

public class BookImage
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public string? Alt { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
}