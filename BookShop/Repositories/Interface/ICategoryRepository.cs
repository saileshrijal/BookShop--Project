﻿using BookShop.Models;

namespace BookShop.Repositories.Interface;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetWithBooks();
}