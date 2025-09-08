using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using demo.Models;
using Microsoft.AspNetCore.Mvc;
using demo.Data;
using Microsoft.EntityFrameworkCore;
using demo.ViewModels;

namespace demo.Controllers
{
    public class UserProfile : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
