namespace BookShop.PartialViewModels;

public class BookInfo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public List<string>? CategoryNames { get; set; }
}