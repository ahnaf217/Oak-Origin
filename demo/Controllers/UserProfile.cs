using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    public class UserProfile : Controller
    {
        public IActionResult User_profile()
        {
            return View();
        }
    }
}
