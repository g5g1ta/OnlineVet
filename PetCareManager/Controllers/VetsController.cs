using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetCareManager.Data;
using PetCareManager.Models;
using PetCareManager.ViewModels;

namespace PetCareManager.Controllers
{
    [Authorize]
    public class VetsController : Controller 
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public VetsController(ApplicationDbContext context, UserManager<User> userManager,
        RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            
            var usersInVetRole = await _userManager.GetUsersInRoleAsync("vet");

            
            var vets = usersInVetRole.Select(user => new VetScheduleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                AvailableFrom = _context.VetSchedules
                                         .Where(s => s.VetId == user.Id)
                                         .Select(s => s.AvailableFrom)
                                         .FirstOrDefault(),
                AvailableTo = _context.VetSchedules
                                       .Where(s => s.VetId == user.Id)
                                       .Select(s => s.AvailableTo)
                                       .FirstOrDefault(),
                IsAvailable = _context.VetSchedules
                                      .Where(s => s.VetId == user.Id)
                                      .Select(s => s.IsAvailable)
                                      .FirstOrDefault(),
                PhoneNumber = user.PhoneNumber
            }).ToList();          
            return View(vets);
        }
    }
}