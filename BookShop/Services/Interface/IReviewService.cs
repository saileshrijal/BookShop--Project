using BookShop.Dtos.ReviewDto;

namespace BookShop.Services.Interface;

public interface IReviewService
{
    Task AddReviewAsync(AddReviewDto review);
}