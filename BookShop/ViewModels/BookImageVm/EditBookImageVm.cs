using System.ComponentModel.DataAnnotations;
using BookShop.PartialViewModels;

namespace BookShop.ViewModels.BookImageVm;

public class EditBookImageVm
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please enter a name")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Please enter a alt")]
    public string? Alt { get; set; }
    [Required(ErrorMessage = "Please enter a display order")]
    public int DisplayOrder { get; set; }
    public int BookId { get; set; }
    public IFormFile? Image { get; set; }
    public string? Path { get; set; }
    public BookInfo? Book { get; set; }
}