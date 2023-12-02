using BookShop.Dtos.UserAddressDto;

namespace BookShop.Services.Interface;

public interface IUserAddressService
{
    Task AddAsync(AddUserAddressDto addUserAddressDto);
    Task EditAsync(EditUserAddressDto editUserAddressDto);
}