namespace BookShop.Models;

public class BaseModel
{
    public int Id { get; set; }
    public bool Status { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}