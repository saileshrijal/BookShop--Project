namespace BookShop.Dtos.BlogDto;

public class EditBlogDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? ThumbnailUrl { get; set; }
}