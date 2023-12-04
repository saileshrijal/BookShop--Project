namespace BookShop.Models;

public class Wishlist
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public string? ApplicationuserId { get; set; }
    public ApplicationUser? Applicationuser { get; set; }
    public DateTime AddedDateTime { get; set; }
}