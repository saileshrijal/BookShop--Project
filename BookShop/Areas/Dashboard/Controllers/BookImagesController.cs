using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Dtos.BookImageDto;
using BookShop.Helpers.Interface;
using BookShop.PartialViewModels;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.BookImageVm;
using BookShop.ViewModels.BookVm;
using Microsoft.AspNetCore.Mvc;
using BookImageVm = BookShop.ViewModels.BookImageVm.BookImageVm;

namespace BookShop.Areas.Dashboard.Controllers;

[Area("Dashboard")]
public class BookImagesController : Controller
{
    private readonly IBookImageRepository _bookImageRepository;
    private readonly IBookImageService _bookImageService;
    private readonly IBookRepository _bookRepository;
    private readonly INotyfService _notyfService;
    private readonly IFileHelper _fileHelper;

    public BookImagesController(IBookImageRepository bookImageRepository,
        IBookImageService bookImageService,
        IBookRepository bookRepository,
        INotyfService notyfService,
        IFileHelper fileHelper)
    {
        _bookImageRepository = bookImageRepository;
        _bookImageService = bookImageService;
        _bookRepository = bookRepository;
        _notyfService = notyfService;
        _fileHelper = fileHelper;
    }

    public async Task<IActionResult> Index(int bookId)
    {
        var book = await _bookRepository.GetWithCategoryByIdAsync(bookId);

        if (book is null)
        {
            _notyfService.Error("Book not found");
            return RedirectToAction(nameof(System.Index), "Books");
        }

        var bookImages = await _bookImageRepository
            .FindByAsync(x => x.BookId == bookId);

        var vm = new BookImageIndexVm
        {
            BookId = bookId,
            Book = new BookInfo()
            {
                Id = book.Id,
                Name = book.Name,
                ShortDescription = book.ShortDescription,
                CategoryName = book.Category?.Name
            },
            BookImages = bookImages.Select(x => new BookImageVm
            {
                Id = x.Id,
                Name = x.Name,
                Path = x.Path,
                Alt = x.Alt,
                DisplayOrder = x.DisplayOrder,
                BookId = x.BookId
            }).ToList()
        };

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Add(int bookId)
    {
        var book = await _bookRepository.GetWithCategoryByIdAsync(bookId);
        var vm = new AddBookImageVm
        {
            BookId = bookId,
            Book = new BookInfo()
            {
                Id = book.Id,
                Name = book.Name,
                ShortDescription = book.ShortDescription,
                CategoryName = book.Category?.Name
            }
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddBookImageVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);
            var dto = new AddBookImageDto
            {
                Name = vm.Name,
                Alt = vm.Alt,
                DisplayOrder = vm.DisplayOrder,
                BookId = vm.BookId
            };
            if (vm.Image != null) dto.Path = await _fileHelper.UploadFileAsync(vm.Image, "books");
            await _bookImageService.AddAsync(dto);
            _notyfService.Success("Book image added successfully");
            return RedirectToAction("Index", new { bookId = vm.BookId });
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return View(vm);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, int bookId)
    {
        var bookImage = await _bookImageRepository.GetByIdAsync(id);
        if (bookImage is null)
        {
            _notyfService.Error("Book image not found");
            return RedirectToAction(nameof(Index), "BookImages", new { bookId });
        }
        var book = await _bookRepository.GetWithCategoryByIdAsync(bookId);
        var vm = new EditBookImageVm
        {
            Id = bookImage.Id,
            Name = bookImage.Name,
            Alt = bookImage.Alt,
            DisplayOrder = bookImage.DisplayOrder,
            BookId = bookImage.BookId,
            Path = bookImage.Path,
            Book = new BookInfo()
            {
                Id = book.Id,
                Name = book.Name,
                ShortDescription = book.ShortDescription,
                CategoryName = book.Category?.Name
            }
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditBookImageVm vm)
    {
        try
        {
            if (!ModelState.IsValid) return View(vm);

            var dto = new EditBookImageDto
            {
                Id = vm.Id,
                Name = vm.Alt,
                DisplayOrder = vm.DisplayOrder,
                BookId = vm.BookId,
                Path = vm.Path
            };

            if (vm.Image != null)
            {
                dto.Path = await _fileHelper.UploadFileAsync(vm.Image, "books");
                if (!string.IsNullOrWhiteSpace(vm.Path))
                    await _fileHelper.DeleteFileAsync(vm.Path, "books");
            }

            await _bookImageService.EditAsync(dto);
            _notyfService.Success("Book image edited successfully");
            return RedirectToAction(nameof(Index), new { bookId = vm.BookId });
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return View(vm);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, int bookId)
    {
        try
        {
            var bookImage = await _bookImageRepository.GetByIdAsync(id);
            if (bookImage is null)
            {
                _notyfService.Error("Book image not found");
                return RedirectToAction(nameof(Index), "BookImages", new { bookId });
            }
            await _bookImageService.DeleteAsync(id);
            if (!string.IsNullOrWhiteSpace(bookImage.Path))
                await _fileHelper.DeleteFileAsync(bookImage.Path, "books");
            _notyfService.Success("Book image deleted successfully");
            return RedirectToAction(nameof(Index), "BookImages", new { bookId });
        }
        catch (Exception e)
        {
            _notyfService.Error(e.Message);
            return RedirectToAction(nameof(Index), "BookImages", new { bookId });
        }
    }
}