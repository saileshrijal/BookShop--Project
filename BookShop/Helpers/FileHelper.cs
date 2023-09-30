using System.Transactions;
using BookShop.Helpers.Interface;

namespace BookShop.Helpers;

public class FileHelper : IFileHelper
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileHelper(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public Task<string> DeleteFileAsync(string fileName, string folderName)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folderName);
        var filePath = Path.Combine(directoryPath, fileName);
        if (File.Exists(filePath)) File.Delete(filePath);
        tx.Complete();
        return Task.FromResult(fileName);
    }

    public async Task<string> UploadFileAsync(IFormFile file, string folderName)
    {
        using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var directoryPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folderName);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(directoryPath, fileName);
        await using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
        tx.Complete();
        return fileName;
    }
}