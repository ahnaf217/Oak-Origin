using demo.Data;
using demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace demo.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public readonly UserManager<Users> _userManager;
        public readonly AppDbContext _context;

        public CartController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Cart_Index()
        {
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            var cart = await _context.Carts
                .Include(x =>x.Product)
                .Where(x => x.UserId == currentuser.Id)
                .ToListAsync();

            decimal totalCost = 0;
             foreach (var cartItem in cart) {
                    totalCost += cartItem.Product.Price * cartItem.Qty;
            }
             ViewBag.TotalCost = totalCost;

            return View(cart);
        }
        public async Task<IActionResult> AddToCart(int productId, int qty = 1)
        {
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);
            var product = await _context.Products.Where(x => x.Id == productId).FirstOrDefaultAsync();
            if(product == null)
            {
                return BadRequest();
            }
            var cart = new Cart { ProductId = productId, Qty = qty, UserId = currentuser.Id };
            _context.Add(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Cart_Index");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var cartItem = await _context.Carts.FindAsync(id);
            if(cartItem == null)
            {
                return BadRequest();
            }
            _context.Carts.Remove(cartItem);
            await _context.SaveChangesAsync(); 
            return RedirectToAction("Cart_Index");
        }

        /*[HttpPost]
        public IActionResult UpdateQuantity(int cartItemId, int quantity, string change)
        {
            var cartItem = _context.Carts
                .Include(c => c.Product)
                .FirstOrDefault(c => c.Id == cartItemId);

            if (cartItem != null)
            {
                if (change == "increase")
                    cartItem.Qty++;
                else if (change == "decrease" && cartItem.Qty > 1)
                    cartItem.Qty--;
                else
                    cartItem.Qty = quantity;

                // Update total in DB if you store it
                cartItem.total = (int)(cartItem.Product.Price * cartItem.Qty);

                _context.SaveChanges();
            }

            return RedirectToAction("Cart_Index"); // reloads the cart page with new totals*
        }*/

        [HttpPost]
        public async Task<IActionResult> UpdateCart(List<Cart> CartItems)
        {
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            foreach (var item in CartItems)
            {
                var existing = await _context.Carts
                    .Include(c => c.Product)
                    .FirstOrDefaultAsync(c => c.Id == item.Id && c.UserId == currentuser.Id);

                if (existing != null)
                {
                    existing.Qty = item.Qty > 0 ? item.Qty : 1; // prevent zero/negative
                }
            }

            await _context.SaveChangesAsync();

            // Recalculate total cost
            var cart = await _context.Carts
                .Include(x => x.Product)
                .Where(x => x.UserId == currentuser.Id)
                .ToListAsync();

            decimal totalCost = cart.Sum(c => c.Product.Price * c.Qty);
            ViewBag.TotalCost = totalCost;

            return View("Cart_Index", cart);
        }


    }

}
