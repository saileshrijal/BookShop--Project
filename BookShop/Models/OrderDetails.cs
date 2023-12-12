using BookShop.Enum;

namespace BookShop.Models;

public class OrderDetails
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order? Order { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? TrackingNumber { get; set; }
    public string? SessionId { get; set; }
    public string? PaymentIntentId { get; set; }
    public DateTime DateOfOrderApproved{ get; set; }
    public DateTime DateOfPayment { get; set; }
    public DateTime DateOfOrderCancelled { get; set; }
    public DateTime DateOfOrderShipped { get; set; }
    public DateTime DateOfOrderDelivered { get; set; }
}