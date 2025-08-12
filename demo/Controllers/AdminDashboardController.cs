using Microsoft.AspNetCore.Mvc;
using demo.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace demo.Controllers
{
    public class AdminDashboardController : Controller
    {
        [Authorize(Roles = "Admin")]
        public IActionResult Admin_Dashboard()
        {
            ViewData["HideHeaderFooter"] = true;
            return View();
        }
    }
}
