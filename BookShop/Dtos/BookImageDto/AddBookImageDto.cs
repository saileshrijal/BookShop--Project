namespace BookShop.Dtos.BookImageDto;

public class AddBookImageDto
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? Alt { get; set; }
    public int DisplayOrder { get; set; }
    public int BookId { get; set; }
}