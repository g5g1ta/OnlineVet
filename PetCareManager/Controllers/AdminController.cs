using System.Collections.Immutable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareManager.Data;
using PetCareManager.Models;
using PetCareManager.ViewModels;

namespace PetCareManager.Controllers
{

    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        public AdminController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AllVets(bool? isAvailable, bool hasSchedule)
        {
            var vets = await _userManager.GetUsersInRoleAsync("Vet");
          
            var vetSchedules = await _context.VetSchedules
                .Include(v => v.Vet)
                .Where(v => vets.Select(u => u.Id).Contains(v.VetId))
                .ToListAsync();
            if (isAvailable.HasValue)
            {
                vetSchedules = vetSchedules
                    .Where(v => v.IsAvailable == isAvailable.Value)
                    .ToList();
            }
            return View(vetSchedules);
        

        }
        [HttpGet]
        public async Task<IActionResult> VetsWithoutSchedule()
        {
            var vets = await _userManager.GetUsersInRoleAsync("Vet");

            var vetsWithoutSchedule = vets
                    .Where(u => !_context.VetSchedules.Any(vs => vs.VetId == u.Id))
                    .ToList();
            return View(vetsWithoutSchedule);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var schedule = await _context.VetSchedules
                .Include(v => v.Vet)
                .FirstOrDefaultAsync(v => v.VetId == id);

            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VetSchedule updatedSchedule)
        {

            var schedule = await _context.VetSchedules
                .FirstOrDefaultAsync(v => v.VetId == updatedSchedule.VetId);

            if (schedule == null)
            {
                return NotFound();
            }


            schedule.AvailableFrom = updatedSchedule.AvailableFrom;
            schedule.AvailableTo = updatedSchedule.AvailableTo;
            schedule.IsAvailable = updatedSchedule.IsAvailable;

            _context.VetSchedules.Update(schedule);
            await _context.SaveChangesAsync();

            return RedirectToAction("AdminPanel");
        }
        [HttpGet]
        public async Task<IActionResult> AllUsers()
        {
            var CurrentUser = await _userManager.GetUserAsync(User);
            var users = await _context.Users.Where(x => x.Email != CurrentUser.Email).ToListAsync();

            var userRolesViewModel = new List<UserViewModel>();
            foreach (var user in users)
            {
                var thisUserRoles = await _userManager.GetRolesAsync(user);
                var role = thisUserRoles.ToList().First().ToString();
                userRolesViewModel.Add(new UserViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Username = user.UserName,
                    Role = role,
                });
            }
            return View(userRolesViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> ChangeRole(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var userRole = await _userManager.GetRolesAsync(user);
            var role = userRole.ToList().First().ToString();
            var model = new UserViewModel
            {
                UserId = id,
                Username = user.UserName,
                Email = user.Email,
                Role = role,

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangeRole(UserViewModel viewModel)
        {
            var originalUser = await _userManager.FindByIdAsync(viewModel.UserId.ToString());
            var roleList = await _userManager.GetRolesAsync(originalUser);
            var role = roleList.ToList().First().ToString();
            if (originalUser == null)
            {
                return NotFound();
            }
            originalUser.UserName = viewModel.Username;
            await _userManager.RemoveFromRoleAsync(originalUser, role);
            await _userManager.AddToRolesAsync(originalUser, new List<string> { viewModel.Role });

            if (role == "Vet")
            {
                var schedule = _context.VetSchedules.Where(x => x.VetId == originalUser.Id).ToList().First();
                _context.VetSchedules.Remove(schedule);
                _context.SaveChangesAsync();
                return RedirectToAction("AllUsers");

            }
            return RedirectToAction("AllVets");
        }
        [HttpGet]
        public async Task<IActionResult> AddSchedule(int vetId)
        {
            var vet = await _context.Users.FirstOrDefaultAsync(u => u.Id == vetId);
            if (vet == null)
            {
        
                return NotFound();
            }

    
            var model = new ScheduleViewModel
            {
                VetId = vetId
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddSchedule(ScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var vet = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.VetId);
            if (vet == null)
            {
        
                return NotFound();
            } 
            var schedule = new VetSchedule
            {
                VetId = model.VetId,
                AvailableFrom = model.AvailableFrom,
                AvailableTo = model.AvailableTo,
                IsAvailable = model.IsAvailable
            };

            _context.VetSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            return RedirectToAction("AllVets");
        }
        
    }
}