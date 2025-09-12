using Microsoft.AspNetCore.Mvc;

namespace demo.Controllers
{
    public class Blog : Controller
    {
        public IActionResult blog()
        {
            return View();
        }
    }
}
