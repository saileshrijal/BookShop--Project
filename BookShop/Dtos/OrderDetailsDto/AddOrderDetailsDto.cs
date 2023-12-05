using BookShop.Enum;

namespace BookShop.Dtos.OrderDetailsDto;

public class AddOrderDetailsDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public decimal Price { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public string? TrackingNumber { get; set; }
    public int Quantity { get; set; }
    public string? SessionId { get; set; }
    public string? PaymentIntentId { get; set; }
    public DateTime DateOfPayment { get; set; }
    public DateTime DateOfDelivered { get; set; }
}