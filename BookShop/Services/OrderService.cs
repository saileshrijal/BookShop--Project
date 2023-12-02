using BookShop.Dtos.OrderDto;
using BookShop.Enum;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task AddAsync(AddOrderDto addOrderDto)
    {
        var order = new Order()
        {
            ApplicationUserId = addOrderDto.ApplicationUserId,
            DateOfOrder = addOrderDto.DateOfOrder,
            OrderTotal = addOrderDto.OrderTotal,
            OrderStatus = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending,
            SessionId = addOrderDto.SessionId,
            PaymentIntentId = addOrderDto.PaymentIntentId,
            DateOfPayment = addOrderDto.DateOfPayment,
            DueDate = addOrderDto.DueDate,
            TrackingNumber = addOrderDto.TrackingNumber,
            Carrier = addOrderDto.Carrier,
        };
        await _unitOfWork.AddAsync(order);
        order.OrderDetails = addOrderDto.OrderDetails.Select(x => new OrderDetails()
        {
            BookId = x.BookId,
            OrderId = order.Id,
            Price = x.Price,
            Quantity = x.Quantity,
        }).ToList();
        await _unitOfWork.SaveAsync();
    }
}