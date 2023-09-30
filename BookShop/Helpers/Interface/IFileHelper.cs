namespace BookShop.Helpers.Interface;

public interface IFileHelper
{
    Task<string> UploadFileAsync(IFormFile file, string folderName);
    Task<string> DeleteFileAsync(string fileName, string folderName);
}