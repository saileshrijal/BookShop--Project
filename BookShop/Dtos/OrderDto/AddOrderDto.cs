using BookShop.Dtos.OrderDetailsDto;
using BookShop.Enum;

namespace BookShop.Dtos.OrderDto;

public class AddOrderDto
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; }
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
    public List<AddOrderDetailsDto>? OrderDetails { get; set; } = new();
}