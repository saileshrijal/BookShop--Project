namespace BookShop.Dtos.CartDto;

public class AddCartDto
{
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public string? ApplicationUserId { get; set; }
}