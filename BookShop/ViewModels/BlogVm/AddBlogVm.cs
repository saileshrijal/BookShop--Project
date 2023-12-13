namespace BookShop.ViewModels.BlogVm;

public class AddBlogVm
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public IFormFile? Thumbnail { get; set; }
    public string? Slug { get; set; }
    public string? ApplicationUserId { get; set; }
}