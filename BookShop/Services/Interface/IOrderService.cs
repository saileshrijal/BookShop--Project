using BookShop.Dtos.OrderDto;
using BookShop.Enum;

namespace BookShop.Services.Interface;

public interface IOrderService
{
    Task AddAsync(AddOrderDto addOrderDto);
    Task<int> AddAndReturnIdAsync(AddOrderDto addOrderDto);
    Task PayAsync(int orderId, string sessionId, string paymentIntentId);
    Task UpdateOrderStatusAsync(int orderId, OrderStatus orderStatus, PaymentStatus paymentStatus);
}