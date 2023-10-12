using BookShop.Dtos.BookDto;
using BookShop.Dtos.BookImageDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class BookImageService : IBookImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookImageRepository _bookImageRepository;

    public BookImageService(IUnitOfWork unitOfWork, 
        IBookImageRepository bookImageRepository)
    {
        _unitOfWork = unitOfWork;
        _bookImageRepository = bookImageRepository;
    }

    public async Task AddAsync(AddBookImageDto addBookDto)
    {
        var bookImage = new BookImage()
        {
            BookId = addBookDto.BookId,
            Alt = addBookDto.Alt,
            DisplayOrder = addBookDto.DisplayOrder,
            Name = addBookDto.Name,
            Path = addBookDto.Path
        };
        await _unitOfWork.AddAsync(bookImage);
        await _unitOfWork.SaveAsync();
    }

    public async Task EditAsync(EditBookImageDto editBookDto)
    {
        var bookImage = new BookImage()
        {
            Id = editBookDto.Id,
            BookId = editBookDto.BookId,
            Alt = editBookDto.Alt,
            DisplayOrder = editBookDto.DisplayOrder,
            Name = editBookDto.Name,
            Path = editBookDto.Path
        };
        await _unitOfWork.UpdateAsync(bookImage);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var bookImage = await _bookImageRepository.GetByIdAsync(id);
        await _unitOfWork.DeleteAsync(bookImage);
        await _unitOfWork.SaveAsync();
    }
}