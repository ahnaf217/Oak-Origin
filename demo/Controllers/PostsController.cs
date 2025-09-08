using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demo.Data;

namespace demo.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                                      .Include(p => p.Category)
                                      .Where(p => p.IsPublished)
                                      .ToListAsync();
            return View(posts);
        }

        public async Task<IActionResult> Details(string slug)
        {
            if (slug == null) return NotFound();

            var post = await _context.Posts
                                     .Include(p => p.Category)
                                     .FirstOrDefaultAsync(p => p.Slug == slug);

            if (post == null) return NotFound();

            return View(post);
        }
    }
}
