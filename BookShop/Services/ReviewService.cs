using BookShop.Dtos.ReviewDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReviewService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task AddReviewAsync(AddReviewDto addReviewDto)
    {
        var review = new Review()
        {
            ApplicationUserId = addReviewDto.ApplicationUserId,
            BookId = addReviewDto.BookId,
            Rating = addReviewDto.Rating,
            Comment = addReviewDto.Comment
        };
        await _unitOfWork.AddAsync(review);
        await _unitOfWork.SaveAsync();
    }
}