using BookShop.Dtos.UnitDto;

namespace BookShop.Services.Interface;

public interface IUnitService
{
    Task AddAsync(AddUnitDto addUnitDto);
    Task EditAsync(EditUnitDto editUnitDto);
    Task DeleteAsync(int id);
    Task ToggleStatusAsync(int id);
}