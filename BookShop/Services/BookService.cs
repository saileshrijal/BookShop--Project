using System.Transactions;
using BookShop.Dtos.BookDto;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookRepository _bookRepository;

    public BookService(IUnitOfWork unitOfWork,
        IBookRepository bookRepository)
    {
        _unitOfWork = unitOfWork;
        _bookRepository = bookRepository;
    }

    public async Task AddAsync(AddBookDto addBookDto)
    {
        var book = new Book()
        {
            Name = addBookDto.Name,
            Description = addBookDto.Description,
            Price = addBookDto.Price,
            CategoryId = addBookDto.CategoryId,
            FeaturedImage = addBookDto.FeaturedImage,
        };
        await _unitOfWork.AddAsync(book);
        await _unitOfWork.SaveAsync();
    }

    public async Task EditAsync(EditBookDto editBookDto)
    {
        var book = new Book()
        {
            Id = editBookDto.Id,
            Name = editBookDto.Name,
            ShortDescription = editBookDto.ShortDescription,
            Description = editBookDto.Description,
            Price = editBookDto.Price,
            CategoryId = editBookDto.CategoryId,
            FeaturedImage = editBookDto.FeaturedImage,
        };
        await _unitOfWork.UpdateAsync(book);
        await _unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if(book == null) throw new Exception("Book not found");
        await _unitOfWork.DeleteAsync(book);
        await _unitOfWork.SaveAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if(book == null) throw new Exception("Book not found");
        book.Status = !book.Status;
        await _unitOfWork.UpdateAsync(book);
        await _unitOfWork.SaveAsync();
    }
}