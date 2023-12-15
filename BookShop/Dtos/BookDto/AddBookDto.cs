﻿namespace BookShop.Dtos.BookDto;

public class AddBookDto
{
    public string? Name { get; set; }
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public List<int>? CategoryIds { get; set; }
}