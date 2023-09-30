using System.ComponentModel.DataAnnotations;

namespace BookShop.ViewModels.UnitVm;

public class AddUnitVm
{
    [Required(ErrorMessage = "Name is required")]
    public string? Name { get; set; }
    public string? Description { get; set; }
}