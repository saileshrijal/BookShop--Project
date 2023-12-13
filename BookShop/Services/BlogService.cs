using BookShop.Dtos.BlogDto;
using BookShop.Helpers.Interface;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringHelper _stringHelper;

    public BlogService(IBlogRepository blogRepository, IUnitOfWork unitOfWork, IStringHelper stringHelper)
    {
        _blogRepository = blogRepository;
        _unitOfWork = unitOfWork;
        _stringHelper = stringHelper;
    }

    public async Task AddAsync(AddBlogDto addBlogDto)
    {
        var blog = new Blog
        {
            Title = addBlogDto.Title,
            Description = addBlogDto.Description,
            ShortDescription = addBlogDto.ShortDescription,
            Slug = _stringHelper.GenerateSlug(addBlogDto.Title),
            ThumbnailUrl = addBlogDto.ThumbnailUrl,
            ApplicationUserId = addBlogDto.ApplicationUserId
        };

        await _unitOfWork.AddAsync(blog);
        await _unitOfWork.SaveAsync();
    }

    public async Task EditAsync(EditBlogDto editBlogDto)
    {
        var blog = await _blogRepository.GetByIdAsync(editBlogDto.Id);
        blog.Title = editBlogDto.Title;
        blog.ShortDescription = editBlogDto.ShortDescription;
        blog.Description = editBlogDto.Description;
        blog.ThumbnailUrl = editBlogDto.ThumbnailUrl;
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var blog = await _blogRepository.GetByIdAsync(id);
        await _unitOfWork.DeleteAsync(blog);
        await _unitOfWork.SaveAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var blog = await _blogRepository.GetByIdAsync(id);
        blog.Status = !blog.Status;
        await _unitOfWork.SaveAsync();
    }
}