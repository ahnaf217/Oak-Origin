using demo.Data;
using demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
    public class CheckoutController : Controller
    {
        public readonly AppDbContext _context;
        public readonly UserManager<Users> _userManager;

        public CheckoutController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }      

        public async Task<IActionResult> Index()
        {
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            var addresses = await _context.Addresses
                .Include(x => x.User)
                .Where(x => x.UserId == currentuser.Id)
                .ToListAsync();

            ViewBag.Addresses = addresses;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Address address)
        {
            if(ModelState.IsValid)
            {                 var currentuser = await _userManager.GetUserAsync(HttpContext.User);
                address.UserId = currentuser.Id;
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(address);
        }
    }
}
