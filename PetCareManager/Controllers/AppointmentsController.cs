using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetCareManager.Models;
using PetCareManager.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace PetCareManager.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public AppointmentsController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<IActionResult> Create(int vetId)
        {
            var user = await _userManager.GetUserAsync(User);
            var pets = await _context.Pets.Where(p => p.OwnerId == user.Id).ToListAsync();
            ViewBag.Pets = pets;

            ViewBag.VetId = vetId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int vetId, int petId, DateTime appointmentDate)
        {
            var user = await _userManager.GetUserAsync(User);

            var appointment = new Appointment
            {
                AppointmentDate = appointmentDate,
                PetId = petId,
                VetId = vetId,
                Status = AppointmentStatus.Pending
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Vet")]
        public async Task<IActionResult> VetDashboard()
        {
            var currentVet = await _userManager.GetUserAsync(User);

            var appointments = await _context.Appointments
                .Include(a => a.Pet)
                .Include(a => a.Vet)
                .Where(a => a.VetId == currentVet.Id)
                .ToListAsync();

            return View(appointments);
        }
        [Authorize(Roles = "Vet")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int appointmentId, AppointmentStatus status)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                return NotFound();

            appointment.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction("VetDashboard");
        }
    }
}
