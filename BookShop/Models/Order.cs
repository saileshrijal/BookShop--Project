using BookShop.Enum;

namespace BookShop.Models;

public class Order
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public DateTime DateOfOrder { get; set; }
    public DateTime DateOfShipping { get; set; }
    public decimal OrderTotal { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }
    public string? SessionId { get; set; }
    public string? PaymentIntentId { get; set; }
    public DateTime DateOfPayment { get; set; }
    public DateTime DueDate { get; set; }
    public List<OrderDetails> OrderDetails { get; set; } = new();
}