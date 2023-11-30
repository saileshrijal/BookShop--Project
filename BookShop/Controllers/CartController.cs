﻿using AspNetCoreHero.ToastNotification.Abstractions;
using BookShop.Models;
using BookShop.Repositories.Interface;
using BookShop.Services.Interface;
using BookShop.ViewModels.BookVm;
using BookShop.ViewModels.CartVm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ICartRepository _cartRepository;
    private readonly INotyfService _notyfService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(
        ICartService cartService, 
        ICartRepository cartRepository, 
        INotyfService notyfService,
        UserManager<ApplicationUser> userManager)
    {
        _cartService = cartService;
        _cartRepository = cartRepository;
        _notyfService = notyfService;
        _userManager = userManager;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var loggedInUser = await _userManager.GetUserAsync(User);
        var cartItems = await _cartRepository.FindByWithBooks(x=>x.ApplicationUserId == loggedInUser.Id);
        var vm = cartItems?.Select(x=> new CartItemVm()
        {
            Id = x.Id,
            BookId = x.BookId,
            Book = new BookDetailsVm
            {
                Id = x.Book.Id,
                Name = x.Book.Name,
                CategoryNames = x.Book.BookCategories?.Select(x => x.Category?.Name).ToList(),
                FeaturedImage = x.Book.FeaturedImagePath,
                Price = x.Book.Price,
                Description = x.Book.Description,
                ShortDescription = x.Book.ShortDescription,
                CreatedDate = x.Book.CreatedDate,
                Slug = x.Book.Slug,
                Status = x.Book.Status,
                BestSeller = x.Book.BestSeller,
                BookImages = x.Book.BookImages?.Select(x => new BookImageVm
                {
                    FileName = x.Path,
                    Alt = x.Alt,
                    DisplayOrder = x.DisplayOrder
                }).ToList()
            },
            Quantity = x.Count
        }).ToList();
        return View(vm);
    }
}