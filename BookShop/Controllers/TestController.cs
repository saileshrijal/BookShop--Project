using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Controllers;

[Authorize]
public class TestController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}