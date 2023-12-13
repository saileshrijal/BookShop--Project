namespace BookShop.Dtos.BlogDto;

public class AddBlogDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? ApplicationUserId { get; set; }
}