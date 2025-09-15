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

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                                     .Include(p => p.Comments)
                                     .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null) return NotFound();

            return View(post);
        }
    }
}
