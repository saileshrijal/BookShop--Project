using System.Linq.Expressions;
using BookShop.Models;

namespace BookShop.Repositories.Interface;

public interface IWishlistRepository : IRepository<Wishlist>
{
    Task<List<Wishlist>> GetWithBooksAsync(Expression<Func<Wishlist, bool>> expression);
}