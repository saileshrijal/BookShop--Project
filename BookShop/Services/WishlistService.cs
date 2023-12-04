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
        var wishlist = await _wishlistRepository.FindByAsync(x =>
            x.ApplicationuserId == addWishlistDto.ApplicationuserId && x.BookId == addWishlistDto.BookId);
        if (wishlist.Any())
        {
            throw new Exception("This book is already in your wishlist");
        }
        await _unitOfWork.AddAsync(new Wishlist
        {
            ApplicationuserId = addWishlistDto.ApplicationuserId,
            BookId = addWishlistDto.BookId
        });
        await _unitOfWork.SaveAsync();
    }
}