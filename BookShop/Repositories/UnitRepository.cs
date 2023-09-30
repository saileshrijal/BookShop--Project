using BookShop.Data;
using BookShop.Models;
using BookShop.Repositories.Interface;

namespace BookShop.Repositories;

public class UnitRepository : Repository<Unit>, IUnitRepository
{
    public UnitRepository(ApplicationDbContext context) : base(context)
    {
    }
}