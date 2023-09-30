using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.CategoryVm;

public class EditCategoryVm
{
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    public string? Description { get; set; }
}