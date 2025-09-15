using demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> blog()
        {
            var posts = await _context.Posts
                                      .OrderByDescending(p => p.PublishedDate)
                                      .ToListAsync();

            return View(posts); // ✅ pass list to the view
        }
    }
}
