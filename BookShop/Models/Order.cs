using BookShop.Enum;

namespace BookShop.Models;

public class Order
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public DateTime DateOfOrder { get; set; }
    public DateTime DateOfPayment { get; set; }
    public decimal OrderTotal { get; set; }
    public List<OrderDetails> OrderDetails { get; set; } = new();
}