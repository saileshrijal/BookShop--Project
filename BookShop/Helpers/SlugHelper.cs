using System.Text.RegularExpressions;
using BookShop.Helpers.Interface;

namespace BookShop.Helpers;

public class SlugHelper : ISlugHelper
{
    public string GenerateSlug(string phrase)
    {
        string str = phrase.ToLower();
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        str = Regex.Replace(str, @"\s+", " ").Trim();
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        str = Regex.Replace(str, @"\s", "-");
        return str;
    }
}