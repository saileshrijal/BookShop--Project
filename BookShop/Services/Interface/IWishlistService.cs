using BookShop.Dtos.WishlistDto;

namespace BookShop.Services.Interface;

public interface IWishlistService
{
    Task ToggleAsync(AddWishlistDto addWishlistDto);
    Task AddAsync(AddWishlistDto addWishlistDto);
    Task RemoveAsync(AddWishlistDto addWishlistDto);
}