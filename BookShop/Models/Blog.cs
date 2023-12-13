namespace BookShop.Models;

public class Blog : BaseModel
{
    public string? Title { get; set; }
    public string? Slug { get; set; }
    public string? Description { get; set; }
    public string? ShortDescription { get; set; }
    public string? ThumbnailUrl { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public string? ApplicationUserId { get; set; }
}