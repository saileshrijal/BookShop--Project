using BookShop.Repositories.Interface;
using BookShop.ViewModels.BlogVm;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

public class BlogsController : Controller
{
    private readonly IBlogRepository _blogRepository;

    public BlogsController(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var blogs = await _blogRepository.GetAllAsync();
        var vm = blogs.Select(x=> new BlogIndexVm()
        {
            Id = x.Id,
            Title = x.Title,
            Slug = x.Slug,
            Description = x.Description,
            ShortDescription = x.ShortDescription,
            ThumbnailUrl = x.ThumbnailUrl,
            CreatedDate = x.CreatedDate,
            AuthorName = x.ApplicationUser?.FullName
        }).ToList();
        return View(vm);
    }
    
    // GET
    [HttpGet("/blog/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        var blog = await _blogRepository.GetWithUserByAsync(x => x.Slug == slug);
        if (blog == null)
        {
            return NotFound();
        }
        var vm = new BlogIndexVm()
        {
            Id = blog.Id,
            Title = blog.Title,
            Description = blog.Description,
            ShortDescription = blog.ShortDescription,
            ThumbnailUrl = blog.ThumbnailUrl,
            CreatedDate = blog.CreatedDate,
            AuthorName = blog.ApplicationUser.FullName
        };
        return View(vm);
    }
}