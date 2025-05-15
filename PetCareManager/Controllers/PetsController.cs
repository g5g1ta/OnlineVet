using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PetCareManager.Models;
using PetCareManager.Data;
using PetCareManager.ViewModels;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var pet = await _context.Pets
            .FirstOrDefaultAsync(p => p.PetId == id && p.OwnerId == userId);  

            if (pet == null)
            {
                return NotFound();  
            }  
            var petViewModel = new PetViewModel
            {
                PetId = pet.PetId,
                Name = pet.Name,
                Breed = pet.Breed,
                DateOfBirth = pet.DateOfBirth,
                Gender = pet.Gender,
                MedicalHistory = pet.MedicalHistory,
            };

        return View(petViewModel);  
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PetViewModel petViewModel)
        {
            if (id <= 0 || !ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
        
                    System.Console.WriteLine(error.ErrorMessage);
                }
                return NotFound();  
            }

            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var pet = await _context.Pets
                .FirstOrDefaultAsync(p => p.PetId == id && p.OwnerId == userId);  

            if (pet == null)
            {
                System.Console.WriteLine("awdawdad");
                 return NotFound();  
            }

        
            pet.Name = petViewModel.Name;
            pet.Breed = petViewModel.Breed;
            pet.DateOfBirth = petViewModel.DateOfBirth;
            pet.Gender = petViewModel.Gender;

            try
            {
            _context.Update(pet); 
            await _context.SaveChangesAsync();  
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pets.Any(p => p.PetId == pet.PetId))
                {
                    return NotFound();  
                }
                else
                {
                    throw;  
                }
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var pet = await _context.Pets
                .FirstOrDefaultAsync(p => p.PetId == id && p.OwnerId == userId);  
            
            if (pet == null)
            {
                System.Console.WriteLine("dwdaw");
                return NotFound();  
            }

            return View(pet);  
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            System.Console.WriteLine("adawdawdawd");
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var pet = await _context.Pets
                .FirstOrDefaultAsync(p => p.PetId == id && p.OwnerId == userId);  

            if (pet == null)
            {
                System.Console.WriteLine("awdawdawdawd");
                return NotFound();  
            }

            _context.Pets.Remove(pet);  
            await _context.SaveChangesAsync();  
            return RedirectToAction(nameof(Index));  
        }
    }
}