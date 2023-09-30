using BookShop.Dtos.UnitDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class UnitService : IUnitService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUnitRepository _unitRepository;

    public UnitService(IUnitOfWork unitOfWork, IUnitRepository unitRepository)
    {
        _unitOfWork = unitOfWork;
        _unitRepository = unitRepository;
    }

    public async Task AddAsync(AddUnitDto addUnitDto)
    {
        var unit = new Unit()
        {
            Name = addUnitDto.Name,
            Description = addUnitDto.Description,
        };
        await _unitOfWork.AddAsync(unit);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var unit = await _unitRepository.GetByIdAsync(id) ?? throw new Exception("Unit not found");
        await _unitOfWork.DeleteAsync(unit);
        await _unitOfWork.SaveAsync();
    }

    public async Task EditAsync(EditUnitDto editUnitDto)
    {
        var unit = await _unitRepository.GetByIdAsync(editUnitDto.Id) ?? throw new Exception("Unit not found");
        unit.Name = editUnitDto.Name;
        unit.Description = editUnitDto.Description;
        await _unitOfWork.UpdateAsync(unit);
        await _unitOfWork.SaveAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var unit = await _unitRepository.GetByIdAsync(id) ?? throw new Exception("Unit not found");
        unit.Status = !unit.Status;
        await _unitOfWork.UpdateAsync(unit);
        await _unitOfWork.SaveAsync();
    }
}