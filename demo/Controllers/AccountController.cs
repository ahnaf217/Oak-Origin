using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using demo.Models;
using demo.ViewModels;
using System.Threading.Tasks;

namespace demo.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(
            SignInManager<Users> signInManager,
            UserManager<Users> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Find user by email
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(
                    user.UserName,  // Use the username for sign in
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false
                );

                if (result.Succeeded)
                {
                    if (await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Admin_Dashboard", "AdminDashboard");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new Users
            {
                FullName = model.Name,
                UserName = model.Email,
                NormalizedUserName = model.Email.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper()
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                var roleExist = await roleManager.RoleExistsAsync("User");
                if (!roleExist)
                {
                    var role = new IdentityRole("User");
                    await roleManager.CreateAsync(role);
                }

                await userManager.AddToRoleAsync(user, "User");
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByNameAsync(model.Email);

            if(user == null)
            {
                ModelState.AddModelError("", "User not found!");
                return View(model);
            }
            else
            {
                return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
            }
        }
        [HttpGet]
        public IActionResult ChangePassword(string username)
        {
            if(string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Something went wrong");
            }

            var user = await userManager.FindByNameAsync(model.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "User not found");
                return View(model);
            }

            var result = await userManager.RemovePasswordAsync(user);
            if(result.Succeeded)
            {
                result = await userManager.AddPasswordAsync(user, model.NewPassword);
                return RedirectToAction("Login", "Account");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
