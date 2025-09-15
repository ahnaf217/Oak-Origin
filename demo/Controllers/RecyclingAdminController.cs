using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demo.Data;

namespace demo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RecyclingAdminController : Controller
    {
        private readonly AppDbContext _context;

        public RecyclingAdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /RecyclingAdmin/Index
        public async Task<IActionResult> Index()
        {
            var requests = await _context.RecyclingRequests
                                         .Include(r => r.Product)
                                         .Include(r => r.User)
                                         .OrderByDescending(r => r.RequestDate)
                                         .ToListAsync();
            return View(requests);
        }

        // POST: /RecyclingAdmin/Approve/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.RecyclingRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            // Only process pending requests
            if (request.Status != "Pending")
            {
                TempData["ErrorMessage"] = "This request has already been processed.";
                return RedirectToAction("Index");
            }

            // Generate a unique store credit code
            request.StoreCreditCode = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
            request.Status = "Approved";

            _context.Update(request);
            await _context.SaveChangesAsync();

            // Optional: Send an email notification to the user
            // You would use an email service here to notify the user of the approval and credit code.

            TempData["SuccessMessage"] = $"Request approved. Store credit code '{request.StoreCreditCode}' generated.";
            return RedirectToAction("Index");
        }

        // POST: /RecyclingAdmin/Decline/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Decline(int id)
        {
            var request = await _context.RecyclingRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            if (request.Status != "Pending")
            {
                TempData["ErrorMessage"] = "This request has already been processed.";
                return RedirectToAction("Index");
            }

            request.Status = "Declined";
            _context.Update(request);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Request has been declined.";
            return RedirectToAction("Index");
        }

        // POST: /RecyclingAdmin/MarkAsCompleted/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            var request = await _context.RecyclingRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            if (request.Status != "Approved")
            {
                TempData["ErrorMessage"] = "Only approved requests can be marked as completed.";
                return RedirectToAction("Index");
            }

            request.Status = "Completed";
            _context.Update(request);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Request has been marked as completed.";
            return RedirectToAction("Index");
        }
    }
}
