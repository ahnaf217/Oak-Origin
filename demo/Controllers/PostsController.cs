using demo.Data;
using demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace demo.Controllers
{
    public class PostsController : Controller
    {
        private readonly AppDbContext _context;
        

        public PostsController(AppDbContext context)
        {
            _context = context;
            
        }

        // GET: Posts (List all posts)
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                                      .OrderByDescending(p => p.PublishedDate)
                                      .ToListAsync();
            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts
                                     .Include(p => p.Comments)
                                     .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null) return NotFound();

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post, IFormFile FeatureImage)
        {
            if (ModelState.IsValid)
            {
                if (FeatureImage == null)
                {
                    ModelState.AddModelError(nameof(post.FeatureImagePath), "Image is required");
                    return View(post);
                }

                // Ensure folder exists
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Posts");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Save file
                var imageName = Guid.NewGuid() + Path.GetExtension(FeatureImage.FileName);
                var savePath = Path.Combine(uploadFolder, imageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await FeatureImage.CopyToAsync(stream);
                }

                // Save relative path in DB
                post.FeatureImagePath = $"/img/Posts/{imageName}";
                post.PublishedDate = DateTime.Now;

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();

            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post, IFormFile FeatureImage)
        {
            if (id != post.Id) return NotFound();

    if (ModelState.IsValid)
    {
        try
        {
            var existingPost = await _context.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (existingPost == null) return NotFound();

            if (FeatureImage != null)
            {
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "Posts");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var imageName = Guid.NewGuid() + Path.GetExtension(FeatureImage.FileName);
                var savePath = Path.Combine(uploadFolder, imageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await FeatureImage.CopyToAsync(stream);
                }

                // Delete old image
                if (!string.IsNullOrEmpty(existingPost.FeatureImagePath))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingPost.FeatureImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                post.FeatureImagePath = $"/img/Posts/{imageName}";
            }
            else
            {
                // keep old image if no new one uploaded
                post.FeatureImagePath = existingPost.FeatureImagePath;
            }

            post.PublishedDate = existingPost.PublishedDate; // keep original publish date

            _context.Update(post);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Posts.Any(e => e.Id == post.Id))
                return NotFound();
            else
                throw;
        }
        return RedirectToAction(nameof(Index));
    }

    return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var post = await _context.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddComment([FromBody]Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return Json(new 
            { username = comment.UserName,
            commentDate = comment.CommentDate.ToString("yyyy-MM-dd HH:mm"),
            content = comment.Content
            });
        }
    }
}
