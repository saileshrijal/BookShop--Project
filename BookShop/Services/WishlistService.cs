using BookShop.Dtos.WishlistDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class WishlistService : IWishlistService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWishlistRepository _wishlistRepository;

    public WishlistService(IUnitOfWork unitOfWork, IWishlistRepository wishlistRepository)
    {
        _unitOfWork = unitOfWork;
        _wishlistRepository = wishlistRepository;
    }
 
    public async Task ToggleAsync(AddWishlistDto addWishlistDto)
    {
        var wishlist = await _wishlistRepository.GetByAsync(x =>
            x.ApplicationuserId == addWishlistDto.ApplicationuserId && x.BookId == addWishlistDto.BookId);
        if(wishlist == null)
        {
            await AddAsync(addWishlistDto);
        }
        else
        {
            await RemoveAsync(addWishlistDto);
        }
    }

    public async Task AddAsync(AddWishlistDto addWishlistDto)
    {
        var wishlist = new Wishlist
        {
            ApplicationuserId = addWishlistDto.ApplicationuserId,
            BookId = addWishlistDto.BookId
        };
        await _unitOfWork.AddAsync(wishlist);
        await _unitOfWork.SaveAsync();
    }

    public async Task RemoveAsync(AddWishlistDto addWishlistDto)
    {
        var wishlist = await _wishlistRepository.GetByAsync(x =>
            x.ApplicationuserId == addWishlistDto.ApplicationuserId && x.BookId == addWishlistDto.BookId);
        if(wishlist == null) throw new Exception("Wishlist not found");
        await _unitOfWork.DeleteAsync(wishlist);
        await _unitOfWork.SaveAsync();
    }
}