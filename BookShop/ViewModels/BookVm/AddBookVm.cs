using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookShop.ViewModels.BookVm;

public class AddBookVm
{
    [Required(ErrorMessage = "Please enter a name")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "Please enter a short description")]
    [StringLength(150, ErrorMessage = "Short description must be less than 150 characters")]
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    [Required(ErrorMessage = "Please enter a price")]
    public decimal Price { get; set; }
    [Required(ErrorMessage = "Please select a category")]
    public List<int>? CategoryIds { get; set; }
    [Required(ErrorMessage = "Please select a featured image")]
    public IFormFile? FeaturedImage { get; set; }
    public List<SelectListItem>? CategoriesSelectList { get; set; } = new();
}