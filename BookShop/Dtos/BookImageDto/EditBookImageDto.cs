namespace BookShop.Dtos.BookImageDto;

public class EditBookImageDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? Alt { get; set; }
    public int DisplayOrder { get; set; }
    public int BookId { get; set; }
}