using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PetCareManager.Models;
using PetCareManager.Data;
using PetCareManager.ViewModels;

namespace PetCareManager.Controllers
{
    public class PetController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PetController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var pets = _context.Pets
                        .Where(p => p.OwnerId == int.Parse(userId))
                        .ToList();
        
            return View(pets);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Pet pet)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(userId);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            pet.OwnerId = int.Parse(userId);
            ModelState.Remove("Owner");
            ModelState.Remove("OwnerId");

            if (ModelState.IsValid)
            {
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage);
            }

            return View(pet);
        }
    }
}