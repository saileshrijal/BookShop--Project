using AspNetCoreHero.ToastNotification;
using BookShop.Data;
using BookShop.Helpers;
using BookShop.Helpers.Interface;
using BookShop.Repositories;
using BookShop.Repositories.Interface;
using BookShop.Seeder;
using BookShop.Seeder.Interface;
using BookShop.Services;
using BookShop.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace BookShop;

public static class DiConfig
{
    public static IServiceCollection UseBookShop(this IServiceCollection services)
    {
        UseRepo(services);
        UseServices(services);
        UseMisc(services);
        return services;
    }

    private static void UseRepo(IServiceCollection services)
    {
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookImageRepository, BookImageRepository>();
    }

    private static void UseServices(IServiceCollection services)
    {
        services.AddScoped<IUnitService, UnitService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBookImageService, BookImageService>();
    }

    private static void UseMisc(IServiceCollection services)
    {
        services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
        services.BuildServiceProvider().CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>()
            .Database.Migrate();
        services.AddScoped<IUserSeeder, UserSeeder>();
        services.AddScoped<IFileHelper, FileHelper>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISlugHelper, SlugHelper>();
        services.AddScoped<IStringHelper, StringHelper>();
    }
}