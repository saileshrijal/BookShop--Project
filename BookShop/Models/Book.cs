﻿namespace BookShop.Models;

public class Book : BaseModel
{
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool BestSeller { get; set; }
    public string? Slug { get; set; }
    public int Quantity { get; set; }
    public List<BookCategory>? BookCategories { get; set; }
    public List<BookImage>? BookImages { get; set; }
}