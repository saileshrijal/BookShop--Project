using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;

namespace BookShop.Repositories;

public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
{
    public OrderDetailsRepository(ApplicationDbContext context) : base(context)
    {
    }
}