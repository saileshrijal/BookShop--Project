using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;

namespace BookShop.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }
}