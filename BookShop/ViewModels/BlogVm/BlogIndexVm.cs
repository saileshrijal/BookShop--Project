namespace BookShop.ViewModels.BlogVm;

public class BlogIndexVm
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Slug { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? AuthorName { get; set; }
    public bool Status { get; set; }
}