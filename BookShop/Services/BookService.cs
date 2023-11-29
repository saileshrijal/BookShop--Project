using BookShop.Dtos.BookDto;
using BookShop.Helpers.Interface;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;

namespace BookShop.Services;

public class BookService : IBookService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBookRepository _bookRepository;
    private readonly ISlugHelper _slugHelper;

    public BookService(IUnitOfWork unitOfWork,
        IBookRepository bookRepository,
        ISlugHelper slugHelper)
    {
        _unitOfWork = unitOfWork;
        _bookRepository = bookRepository;
        _slugHelper = slugHelper;
    }

    public async Task AddAsync(AddBookDto addBookDto)
    {
        var book = new Book
        {
            Name = addBookDto.Name,
            Description = addBookDto.Description,
            Price = addBookDto.Price,
            FeaturedImagePath = addBookDto.FeaturedImage,
            ShortDescription = addBookDto.ShortDescription,
            BookCategories = addBookDto.CategoryIds?.Select(x => new BookCategory
            {
                CategoryId = x
            }).ToList()
        };
        book.Slug = _slugHelper.GenerateSlug(book.Name + " " + book.Id);
        await _unitOfWork.AddAsync(book);
        await _unitOfWork.SaveAsync();
    }

    public async Task EditAsync(EditBookDto editBookDto)
    {
        var book = await _bookRepository.GetWithCategoryByIdAsync(editBookDto.Id);
        if (book == null) throw new Exception("Book not found");
        book.Name = editBookDto.Name;
        book.ShortDescription = editBookDto.ShortDescription;
        book.Description = editBookDto.Description;
        book.Price = editBookDto.Price;
        if (!string.IsNullOrWhiteSpace(editBookDto.FeaturedImage)) book.FeaturedImagePath = editBookDto.FeaturedImage;
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
        if (book == null) throw new Exception("Book not found");
        await _unitOfWork.DeleteAsync(book);
        await _unitOfWork.SaveAsync();
    }

    public async Task ToggleStatusAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) throw new Exception("Book not found");
        book.Status = !book.Status;
        await _unitOfWork.UpdateAsync(book);
        await _unitOfWork.SaveAsync();
    }
}