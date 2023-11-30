using BookShop.Dtos.CartDto;

namespace BookShop.Services.Interface;

public interface ICartService
{
    Task AddAsync(AddCartDto addCartDto);
    Task EditAsync(EditCartDto editCartDto);
    Task DeleteAsync(int id);
    Task IncrementCountAsync(int id);
    Task DecrementCountAsync(int id);
}