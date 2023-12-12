namespace BookShop.Dtos.ReviewDto;

public class AddReviewDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string? ApplicationUserId { get; set; }
    public string? Comment { get; set; }
    public int Rating { get; set; }
}