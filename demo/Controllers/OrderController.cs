using Microsoft.AspNetCore.Mvc;
using demo.Data;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
    public class OrdersController : Controller
    {
        public readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product)
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);

            
            if (order == null)
            {
                
                return NotFound();
            }
 
            return View(order);
        }
    }
}
