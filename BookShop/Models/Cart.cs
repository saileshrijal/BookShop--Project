namespace BookShop.Models;

public class Cart
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public int Count { get; set; }
}