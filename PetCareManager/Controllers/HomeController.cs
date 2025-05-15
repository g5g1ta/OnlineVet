using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetCareManager.Models;
using PetCareManager.ViewModels;

namespace PetCareManager.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<User> _userManager;

    public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }
        Console.WriteLine("User Email: " + user.Email);
        var roles = await _userManager.GetRolesAsync(user);

        ViewData["Roles"] = roles;
        ViewData["Email"] = user?.Email ?? "No email";

        return View();
    }
    
    public IActionResult Privacy()
    {
        return View();
    }
    public async Task<IActionResult> UserInfo()
    {
        var user = await _userManager.GetUserAsync(User);
        var roles = await _userManager.GetRolesAsync(user);

        var model = new UserInfoViewModel
        {
            Email = user.Email,
            Username = user.UserName,
            Roles = roles.ToList()
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}