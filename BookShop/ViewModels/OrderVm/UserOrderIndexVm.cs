using BookShop.Enum;
using BookShop.Models;
using BookShop.ViewModels.OrderDetailsVm;

namespace BookShop.ViewModels.OrderVm;

public class UserOrderIndexVm
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; }
    public DateTime DateOfOrder { get; set; }
    public DateTime DateOfShipping { get; set; }
    public decimal OrderTotal { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime DateOfPayment { get; set; }
    public List<OrderDetailsIndexVm> OrderDetails { get; set; } = new();
}