namespace BookShop.Dtos.CartDto;

public class EditCartDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public string? ApplicationUserId { get; set; }
}