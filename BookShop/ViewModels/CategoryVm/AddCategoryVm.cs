using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.CategoryVm;

public class AddCategoryVm
{
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    public string? Description { get; set; }
}