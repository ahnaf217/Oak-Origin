using demo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using demo.ViewModels;
using demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace demo.ViewComponents
{
    [ViewComponent(Name = "Navigation")]
    public class Navigation : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public Navigation(AppDbContext context, UserManager<Users> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            if(currentUser == null) {
                var model1 = new NavigationViewModel
                {
                    
                };
                return View("Index", model1);
            }
            var cart = await _context.Carts.Where(x => x.UserId == currentUser.Id).ToListAsync();

            var model = new NavigationViewModel
            {
                Cart = cart
            };

            return View("Index", model);
        }
    }
}