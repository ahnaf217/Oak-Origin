using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using demo.Models;

namespace demo.Controllers
{
    public class AdminAccount : Controller
    {
        private readonly SignInManager<Users> _signInManager;

        public AdminAccount(SignInManager<Users> signInManager)
        {
            _signInManager = signInManager; // Correct assignment
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}