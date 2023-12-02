using BookShop.Dtos.OrderDetailsDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class OrderDetailsService : IOrderDetailsService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderDetailsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task AddAsync(AddOrderDetailsDto addOrderDetailsDto)
    {
        var orderDetails = new OrderDetails()
        {
            OrderId = addOrderDetailsDto.OrderId,
            BookId = addOrderDetailsDto.BookId,
            Price = addOrderDetailsDto.Price,
            Quantity = addOrderDetailsDto.Quantity
        };
        await _unitOfWork.AddAsync(orderDetails);
        await _unitOfWork.SaveAsync();
    }
}
