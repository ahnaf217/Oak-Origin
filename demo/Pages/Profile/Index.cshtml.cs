using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using demo.Models;

namespace demo.Pages.Profile
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public UserProfile UserProfile { get; set; }

        [TempData]
        public string Message { get; set; }

        public void OnGet()
        {
            // Demo data, replace with DB fetch as needed
            UserProfile = new UserProfile
            {
                FirstName = "Leslie",
                LastName = "Cooper",
                Email = "example@gmail.com",
                Phone = "+0123-456-789",
                Gender = "Female"
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // DB update logic here
            Message = "Profile updated successfully!";
            return RedirectToPage();
        }
    }
}
