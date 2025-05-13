using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PetCareManager.Controllers
{
    public class AdminController : Controller
    {
        public AdminController()
        {

        }
        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel(){
            return View();
        }
    }
}