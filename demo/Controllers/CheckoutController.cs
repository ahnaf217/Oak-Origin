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

        public async Task<IActionResult> Confirm(int addressId)
        {
            var address = await _context.Addresses
                .Where(x => x.Id == addressId)
                .FirstOrDefaultAsync();
            if(address == null)
            {
                return BadRequest();
            }
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            decimal orderCost = 0;

            var carts = await _context.Carts
                .Include(x => x.Product)
                .Where(x => x.UserId == currentuser.Id)
                .ToListAsync();
            foreach(var cart in carts)
            {
                orderCost += (cart.Product.Price * cart.Qty);
            }
            var order = new Order
            {
                AddressId = addressId,
                CreatedAt = DateTime.Now,
                Status = "Order Placed",
                UserId = currentuser.Id,
                Amount = (double)orderCost,
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            foreach (var cart in carts)
            {
                var orderProduct = new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = cart.ProductId,
                    Price = cart.Product.Price,
                    Qty = cart.Qty,

                };
                _context.Add(orderProduct);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("ThankYou");
            
        }
        public IActionResult ThankYou()
        {
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
