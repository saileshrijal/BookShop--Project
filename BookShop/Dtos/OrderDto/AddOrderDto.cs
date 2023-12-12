using BookShop.Dtos.OrderDetailsDto;
using BookShop.Enum;

namespace BookShop.Dtos.OrderDto;

public class AddOrderDto
{
    public string? ApplicationUserId { get; set; }
    public DateTime DateOfOrder { get; set; }
    public decimal OrderTotal { get; set; }
    public string? TrackingNumber { get; set; }
    public DateTime DateOfPayment { get; set; }
    public List<AddOrderDetailsDto>? OrderDetails { get; set; } = new();
}