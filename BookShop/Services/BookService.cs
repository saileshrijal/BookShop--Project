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
        var book = new Book
        {
            Name = addBookDto.Name,
            Description = addBookDto.Description,
            Price = addBookDto.Price,
            FeaturedImage = addBookDto.FeaturedImage,
            BookCategories = addBookDto.CategoryIds?.Select(x => new BookCategory()
            {
                CategoryId = x
            }).ToList()
        };

        await _unitOfWork.AddAsync(book);
        await _unitOfWork.SaveAsync();
    }

    public async Task EditAsync(EditBookDto editBookDto)
    {
        var book = await _bookRepository.GetWithCategoryByIdAsync(editBookDto.Id);
        if (book == null)
        {
            throw new Exception("Book not found");
        }
        book.Name = editBookDto.Name;
        book.ShortDescription = editBookDto.ShortDescription;
        book.Description = editBookDto.Description;
        book.Price = editBookDto.Price;
        book.FeaturedImage = editBookDto.FeaturedImage;
        if (editBookDto.CategoryIds != null)
        {
            var updatedCategories = editBookDto.CategoryIds.Select(categoryId => new BookCategory
            {
                BookId = book.Id,
                CategoryId = categoryId
            }).ToList();
            book.BookCategories?.Clear();
            book.BookCategories?.AddRange(updatedCategories);
        }
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