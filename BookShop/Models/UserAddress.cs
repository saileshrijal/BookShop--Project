﻿using BookShop.Enum;

namespace BookShop.Models;

public class UserAddress
{
    public int Id { get; set; }
    public AddressType AddressType { get; set; }
    public string? ApplicationUserId { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
}