using demo.Data;
using demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims; // Make sure to add this using statement

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

        // Pass the cart items to the view to show a summary
        var carts = await _context.Carts.Include(c => c.Product)
            .Where(c => c.UserId == currentuser.Id).ToListAsync();
        ViewBag.CartItems = carts;

        return View();
    }

    // Updated Confirm action with storeCreditCode parameter
    public async Task<IActionResult> Confirm(int addressId, string storeCreditCode)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == addressId);
        if (address == null)
        {
            TempData["ErrorMessage"] = "The selected address is not valid.";
            return RedirectToAction("Index");
        }

        var currentUser = await _userManager.GetUserAsync(HttpContext.User);

        // Calculate the base order cost
        decimal orderCost = 0;
        var carts = await _context.Carts
            .Include(x => x.Product)
            .Where(x => x.UserId == currentUser.Id)
            .ToListAsync();

        if (!carts.Any())
        {
            TempData["ErrorMessage"] = "Your cart is empty.";
            return RedirectToAction("Index");
        }

        foreach (var cart in carts)
        {
            orderCost += cart.Product.Price * cart.Qty;
        }

        // --- Store Credit Logic ---
        decimal discountAmount = 0;
        RecyclingRequest? usedCreditRequest = null;

        if (!string.IsNullOrEmpty(storeCreditCode))
        {
            usedCreditRequest = await _context.RecyclingRequests
                .FirstOrDefaultAsync(r => r.StoreCreditCode == storeCreditCode.Trim() && r.UserId == currentUser.Id);

            if (usedCreditRequest != null && usedCreditRequest.Status == "Approved" && !usedCreditRequest.IsCreditUsed)
            {
                discountAmount = usedCreditRequest.StoreCreditAmount;
                orderCost = Math.Max(0, orderCost - discountAmount); // Ensure cost doesn't go below zero
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid or already used store credit code.";
                return RedirectToAction("Index");
            }
        }

        // Create the order
        var order = new Order
        {
            AddressId = addressId,
            CreatedAt = DateTime.Now,
            Status = "Order Placed",
            UserId = currentUser.Id,
            Amount = (double)orderCost,
            DiscountAmount = (double)discountAmount // Add a new field to your Order model to track the discount
        };

        _context.Orders.Add(order);

        // If a valid store credit was used, mark it as used
        if (usedCreditRequest != null)
        {
            usedCreditRequest.IsCreditUsed = true;
            _context.Update(usedCreditRequest);
        }

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

        // Clear the user's cart after a successful order
        _context.Carts.RemoveRange(carts);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Your order has been placed successfully!";
        return RedirectToAction("ThankYou");
    }

    public IActionResult ThankYou()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(Address address)
    {
        var currentUser = await _userManager.GetUserAsync(HttpContext.User);

        if (ModelState.IsValid)
        {
            address.UserId = currentUser.Id;
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "New address added successfully!";
            return RedirectToAction("Index");
        }

        // Re-load addresses and cart items if the model state is invalid
        var addresses = await _context.Addresses
            .Include(x => x.User)
            .Where(x => x.UserId == currentUser.Id)
            .ToListAsync();
        ViewBag.Addresses = addresses;

        var carts = await _context.Carts.Include(c => c.Product)
            .Where(c => c.UserId == currentUser.Id).ToListAsync();
        ViewBag.CartItems = carts;

        TempData["ErrorMessage"] = "Failed to add new address. Please check your input.";
        return View(address);
    }
}