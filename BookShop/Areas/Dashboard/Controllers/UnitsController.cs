using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Dtos.UnitDto;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.UnitVm;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class UnitsController : Controller
{
    private readonly IUnitRepository _unitRepository;
    private readonly IUnitService _unitService;
    private readonly INotyfService _notyfService;

    public UnitsController(IUnitRepository unitRepository,
        IUnitService unitService,
        INotyfService notyfService)
    {
        _unitRepository = unitRepository;
        _unitService = unitService;
        _notyfService = notyfService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var units = await _unitRepository.GetAllAsync();
            var vm = units.Select(x => new UnitIndexVm()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Status = x.Status,
                CreatedDate = x.CreatedDate
            }).ToList();
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View();
        }
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(AddUnitVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new AddUnitDto()
            {
                Name = vm.Name,
                Description = vm.Description
            };
            await _unitService.AddAsync(dto);
            _notyfService.Success("Unit added successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View(vm);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _unitService.DeleteAsync(id);
            _notyfService.Success("Unit deleted successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var unit = await _unitRepository.GetByIdAsync(id) ?? throw new Exception("Unit not found");
            var vm = new EditUnitVm()
            {
                Id = unit.Id,
                Name = unit.Name,
                Description = unit.Description
            };
            return View(vm);
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditUnitVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new EditUnitDto()
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description
            };
            await _unitService.EditAsync(dto);
            _notyfService.Success("Unit edited successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return View(vm);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        try
        {
            await _unitService.ToggleStatusAsync(id);
            _notyfService.Success("Unit status changed successfully");
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _notyfService.Error(ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}