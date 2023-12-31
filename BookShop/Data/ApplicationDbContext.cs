﻿using BookShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
    public DbSet<Unit>? Units { get; set; }
    public DbSet<Category>? Categories { get; set; }
    public DbSet<Book>? Books { get; set; }
    public DbSet<BookImage>? BookImages { get; set; }
    public DbSet<Cart>? Carts { get; set; }
    public DbSet<UserAddress>? UserAddresses { get; set; }
    public DbSet<Order>? Orders { get; set; }
    public DbSet<OrderDetails>? OrderDetails { get; set; }
    public DbSet<Wishlist>? Wishlists { get; set; }
    public DbSet<Blog>? Blogs { get; set; }
}