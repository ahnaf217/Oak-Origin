using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demo.Data;             
using demo.Models;            

namespace demo.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _db;
        public BlogController(AppDbContext db) => _db = db;

        // /blog?page=1
        public async Task<IActionResult> Index(int page = 1, int pageSize = 9)
        {
            var query = _db.Posts
                .Include(p => p.Category)
                .Where(p => p.IsPublished)
                .OrderByDescending(p => p.PublishedAt);

            var total = await query.CountAsync();
            var posts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            return View(posts);
        }

        // /blog/{slug}
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug)) return NotFound();

            var post = await _db.Posts
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Slug == slug);

            if (post == null) return NotFound();

            var related = await _db.Posts
                .Where(p => p.CategoryId == post.CategoryId && p.Id != post.Id && p.IsPublished)
                .OrderByDescending(p => p.PublishedAt)
                .Take(3)
                .ToListAsync();

            ViewBag.Related = related;
            return View(post);
        }
    }
}
