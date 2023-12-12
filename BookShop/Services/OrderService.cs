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

    public OrderService(IUnitOfWork unitOfWork, 
        IOrderRepository orderRepository,
        IOrderDetailsRepository orderDetailsRepository
        )
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _orderDetailsRepository = orderDetailsRepository;
    }
    
    public async Task AddAsync(AddOrderDto addOrderDto)
    {
        var order = new Order()
        {
            ApplicationUserId = addOrderDto.ApplicationUserId,
            DateOfOrder = addOrderDto.DateOfOrder,
            OrderTotal = addOrderDto.OrderTotal,
        };
        await _unitOfWork.AddAsync(order);
        if (addOrderDto.OrderDetails != null)
            order.OrderDetails = addOrderDto.OrderDetails.Select(x => new OrderDetails()
            {
                BookId = x.BookId,
                OrderId = order.Id,
                Price = x.Price,
                Quantity = x.Quantity,
                Total = x.Total,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                DateOfOrderApproved = DateTime.Now,
                TrackingNumber = addOrderDto.TrackingNumber,
            }).ToList();
        await _unitOfWork.SaveAsync();
    }

    public async Task<int> AddAndReturnIdAsync(AddOrderDto addOrderDto)
    {
        var order = new Order()
        {
            ApplicationUserId = addOrderDto.ApplicationUserId,
            DateOfOrder = DateTime.Now,
            OrderTotal = addOrderDto.OrderTotal,
        };
        if (addOrderDto.OrderDetails != null)
            order.OrderDetails = addOrderDto.OrderDetails.Select(x => new OrderDetails()
            {
                BookId = x.BookId,
                OrderId = order.Id,
                Price = x.Price,
                Quantity = x.Quantity,
                Total = x.Total,
                OrderStatus = OrderStatus.Pending,
                PaymentStatus = PaymentStatus.Pending,
                DateOfOrderApproved = DateTime.Now,
                TrackingNumber = addOrderDto.TrackingNumber,
            }).ToList();
        await _unitOfWork.AddAsync(order);
        await _unitOfWork.SaveAsync();
        return order.Id;
    }

    public async Task PayAsync(int orderId, string sessionId, string paymentIntentId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
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

    public async Task UpdateOrderAndPaymentStatusAsync(int orderDetailsId, OrderStatus orderStatus, PaymentStatus paymentStatus)
    {
        var orderDetails = await _orderDetailsRepository.GetByIdAsync(orderDetailsId);
        orderDetails.OrderStatus = orderStatus;
        orderDetails.PaymentStatus = paymentStatus;
        await _unitOfWork.SaveAsync();
    }

    public async Task UpdateOrderStatusAsync(int orderDetailId, OrderStatus orderStatus)
    {
        var orderDetail = await _orderDetailsRepository.GetByIdAsync(orderDetailId);
        orderDetail.OrderStatus = orderStatus;
        switch (orderStatus)
        {
            case OrderStatus.Delivered:
                orderDetail.DateOfOrderDelivered = DateTime.Now;
                break;
            case OrderStatus.Approved:
                orderDetail.DateOfOrderApproved = DateTime.Now;
                break;
            case OrderStatus.Cancelled:
                orderDetail.DateOfOrderCancelled = DateTime.Now;
                break;
            case OrderStatus.Shipped:
                orderDetail.DateOfOrderShipped = DateTime.Now;
                break;
        }
        await _unitOfWork.SaveAsync();
    }
}