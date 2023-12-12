using BookShop.Enum;
using BookShop.ViewModels.BookVm;

namespace BookShop.ViewModels.OrderDetailsVm;

public class OrderDetailsIndexVm
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public BookDetailsVm? Book { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal Total { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime DateOfPayment { get; set; }
    public DateTime DateOfOrderDelivered { get; set; }
    public DateTime DateOfOrderApproved { get; set; }
    public DateTime DateOfOrderShipped { get; set; }
    public DateTime DateOfOrderCancelled { get; set; }
}