using BookShop.Dtos.OrderDetailsDto;

namespace BookShop.Services.Interface;

public interface IOrderDetailsService
{
    Task AddAsync(AddOrderDetailsDto addOrderDetailsDto);
}