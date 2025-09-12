using System.Diagnostics;
using demo.Models;
using Microsoft.AspNetCore.Mvc;
using demo.Data;
using Microsoft.EntityFrameworkCore;
using demo.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private AppDbContext _context;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            var sliderImages = await _context.SliderImages
                .OrderBy(si => si.SortOrder)
                .ToListAsync();

            var categories = await _context.Categories.ToListAsync();

            var model = new HomeViewModel
            {
                Products = products,
                SliderImages = sliderImages,
                Categories = categories
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Shop(int? categoryid)
        {
            var title = "All Products";


            var productsQuery = _context.Products
                .Include(p => p.Category) // include category data
                .AsQueryable();

            if (categoryid != null)
            {
                productsQuery = productsQuery.Where(p => p.CategoryId == categoryid);

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryid);

                if (category != null)
                {
                    title = $"{category.Name}s";
                }
            }

            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", categoryid);

            ViewBag.Title = title;

            var products = await productsQuery.ToListAsync();

            /*var model = new ProductViewModels
            {
                Products = products
            };*/

            return View(products);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if(product == null)
            {
                return NotFound();
            }

                return View(product);
        }



    }
}
