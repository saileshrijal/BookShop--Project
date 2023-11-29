namespace BookShop.Helpers.Interface;

public interface IStringHelper
{
    string GenerateSlug(string phrase);
    string GenerateCommaSeperatedString(List<string> list);
}