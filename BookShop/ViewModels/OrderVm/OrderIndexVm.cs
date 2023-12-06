using BookShop.Models;
using BookShop.ViewModels.OrderDetailsVm;
using BookShop.ViewModels.UserVm;

namespace BookShop.ViewModels.OrderVm;

public class OrderIndexVm
{
    public int Id { get; set; }
    public string? ApplicationUserId { get; set; }
    public DateTime DateOfOrder { get; set; }
    public DateTime DateOfShipping { get; set; }
    public decimal OrderTotal { get; set; }
    public DateTime DateOfPayment { get; set; }
    public List<OrderDetailsIndexVm> OrderDetails { get; set; } = new();
    public UserIndexVm User { get; set; }
    public UserAddressVm UserAddress { get; set; }
}