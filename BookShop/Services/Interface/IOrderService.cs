using BookShop.Dtos.OrderDto;

namespace BookShop.Services.Interface;

public interface IOrderService
{
    Task AddAsync(AddOrderDto addOrderDto);
}