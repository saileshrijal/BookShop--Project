namespace BookShop.Models;

public class Review
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public string? Comment { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedDateTime { get; set; }
}