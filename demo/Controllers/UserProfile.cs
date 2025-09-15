using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using demo.Models;

namespace demo.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly UserManager<Users> _userManager;

        public UserProfileController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> User_profile()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(user);
        }
    }
}
