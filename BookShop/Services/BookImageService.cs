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
        var bookImage = await _bookImageRepository.GetByIdAsync(editBookDto.Id);
        if (bookImage == null) throw new Exception("Book not found");
        bookImage.BookId = editBookDto.BookId;
        bookImage.Alt = editBookDto.Alt;
        bookImage.DisplayOrder = editBookDto.DisplayOrder;
        bookImage.Name = editBookDto.Name;
        if(!string.IsNullOrWhiteSpace(editBookDto.Path))
        {
            bookImage.Path = editBookDto.Path;
        }
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var bookImage = await _bookImageRepository.GetByIdAsync(id);
        await _unitOfWork.DeleteAsync(bookImage);
        await _unitOfWork.SaveAsync();
    }
}