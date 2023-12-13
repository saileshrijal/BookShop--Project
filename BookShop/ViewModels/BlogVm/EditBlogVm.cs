namespace BookShop.ViewModels.BlogVm;

public class EditBlogVm
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? ThumbnailUrl { get; set; }
    public IFormFile? Thumbnail { get; set; }
    public string? Slug { get; set; }
    public string? ApplicationUserId { get; set; }
}