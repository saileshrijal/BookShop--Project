using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;

namespace BookShop.Repositories;

public class UserAddressRepository : Repository<UserAddress>, IUserAddressRepository
{
    public UserAddressRepository(ApplicationDbContext context) : base(context)
    {
    }
}