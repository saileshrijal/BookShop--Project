using BookShop.Dtos.OrderDto;
using BookShop.Enum;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderDetailsRepository _orderDetailsRepository;
    private readonly IBookRepository _bookRepository;

    public OrderService(IUnitOfWork unitOfWork, 
        IOrderRepository orderRepository,
        IBookRepository bookRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _bookRepository = bookRepository;
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

    public async Task<int> AddAndReturnIdAsync(AddOrderDto addOrderDto)
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
        order.OrderDetails = addOrderDto.OrderDetails.Select(x => new OrderDetails()
        {
            BookId = x.BookId,
            OrderId = order.Id,
            Price = x.Price,
            Quantity = x.Quantity,
        }).ToList();
        await _unitOfWork.AddAsync(order);
        await _unitOfWork.SaveAsync();
        return order.Id;
    }

    public async Task PayAsync(int orderId, string sessionId, string paymentIntentId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        order.OrderStatus = OrderStatus.Processing;
        order.PaymentStatus = PaymentStatus.Approved;
        order.SessionId = sessionId;
        order.PaymentIntentId = paymentIntentId;
        order.DateOfPayment = DateTime.Now;
        foreach (var orderDetails in order.OrderDetails)
        {
            orderDetails.OrderStatus = OrderStatus.Processing;
            orderDetails.PaymentStatus = PaymentStatus.Approved;
            orderDetails.SessionId = sessionId;
            orderDetails.PaymentIntentId = paymentIntentId;
            orderDetails.DateOfPayment = DateTime.Now;
        }
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateOrderStatusAsync(int orderId, OrderStatus orderStatus, PaymentStatus paymentStatus)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        order.OrderStatus = orderStatus;
        order.PaymentStatus = paymentStatus;
        foreach (var orderDetails in order.OrderDetails)
        {
            orderDetails.OrderStatus = orderStatus;
            orderDetails.PaymentStatus = paymentStatus;
        }
        await _unitOfWork.SaveAsync();
    }
}