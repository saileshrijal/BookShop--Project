using System.ComponentModel.DataAnnotations;
using BookShop.PartialViewModels;
using BookShop.ViewModels.BookVm;

namespace BookShop.ViewModels.BookImageVm;

public class AddBookImageVm
{
    [Required(ErrorMessage = "Please enter a name")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Please enter a alt")]
    public string? Alt { get; set; }
    [Required(ErrorMessage = "Please enter a display order")]
    public int DisplayOrder { get; set; }
    public int BookId { get; set; }
    [Required(ErrorMessage = "Please upload a image")]
    public IFormFile? Image { get; set; }
    public BookInfo? Book { get; set; }
}