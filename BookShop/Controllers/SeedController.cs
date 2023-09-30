using BookShop.Seeder.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

public class SeedController : Controller
{
    private readonly IUserSeeder _userSeeder;
    public SeedController(IUserSeeder userSeeder)
    {
        _userSeeder = userSeeder;
    }
    
    public async Task<IActionResult> SeedAdminUser()
    {
        try
        {
            await _userSeeder.SeedAdminUserAsync();
            return Content("Admin user created");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}