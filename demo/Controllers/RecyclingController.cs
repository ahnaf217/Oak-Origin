using demo.Data;
using demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace demo.Controllers
{
    public class RecyclingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public RecyclingController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var pastOrders = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Where(o => o.UserId == currentUser.Id)
                .ToListAsync();

            return View(pastOrders);
        }

        // GET: /Recycling/Request/{productId}
        public async Task<IActionResult> Request(int? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Pass the product details to the view
            ViewBag.Product = product;
            return View(new RecyclingRequest { ProductId = product.Id });
        }

        // POST: /Recycling/SubmitRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitRequest(RecyclingRequest model)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(model.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                model.StoreCreditAmount = product.Price * 0.20m;
                model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(model);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your recycling request has been submitted successfully!";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Product = await _context.Products.FindAsync(model.ProductId);
            return View("Request", model);
        }

        public IActionResult Home()
        {
            return View();
        }
    }
}
