using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetCareManager.Models;
using PetCareManager.ViewModels;

namespace PetCareManager.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly RoleManager<IdentityRole<int>> _roleManager;

         public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(!ModelState.IsValid) return View(model);

            var user = new User {Email = model.Email, UserName = model.Username, PhoneNumber = model.PhoneNumber};

            var result = await _userManager.CreateAsync(user, model.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

    
            var user = await _userManager.FindByEmailAsync(model.Email);
    
    
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (result.Succeeded)
            {
       
                return RedirectToAction("Index", "Home");
            }

   
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Logout(){

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}