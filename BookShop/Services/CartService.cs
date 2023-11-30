using BookShop.Dtos.CartDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class CartService : ICartService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICartRepository _cartRepository;

    public CartService(
        IUnitOfWork unitOfWork,
        ICartRepository cartRepository)
    {
        _unitOfWork = unitOfWork;
        _cartRepository = cartRepository;
    }
    
    public async Task AddAsync(AddCartDto addCartDto)
    {
        var cart = new Cart
        {
            ApplicationUserId = addCartDto.ApplicationUserId,
            BookId = addCartDto.BookId,
            Count = addCartDto.Quantity
        };
        var cartInfo = await _cartRepository.GetByAsync(x=>x.BookId== cart.BookId && x.ApplicationUserId == cart.ApplicationUserId);
        if (cartInfo != null)
        {
            cartInfo.Count += cart.Count;
            await _unitOfWork.UpdateAsync(cartInfo);
            await _unitOfWork.SaveAsync();
            return;
        }
        await _unitOfWork.AddAsync(cart);
        await _unitOfWork.SaveAsync();
    }

    public async Task EditAsync(EditCartDto editCartDto)
    {
        var cart = new Cart
        {
            Id = editCartDto.Id,
            ApplicationUserId = editCartDto.ApplicationUserId,
            BookId = editCartDto.BookId,
            Count = editCartDto.Quantity
        };
        await _unitOfWork.UpdateAsync(cart);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var cart = await _cartRepository.GetByIdAsync(id);
        await _unitOfWork.DeleteAsync(cart);
        await _unitOfWork.SaveAsync();
    }

    public async Task IncrementCountAsync(int id)
    {
        var cart = await _cartRepository.GetByIdAsync(id);
        cart.Count++;
        await _unitOfWork.UpdateAsync(cart);
        await _unitOfWork.SaveAsync();
    }

    public async Task DecrementCountAsync(int id)
    {
        var cart = await _cartRepository.GetByIdAsync(id);
        cart.Count--;
        await _unitOfWork.UpdateAsync(cart);
        await _unitOfWork.SaveAsync();
    }
}