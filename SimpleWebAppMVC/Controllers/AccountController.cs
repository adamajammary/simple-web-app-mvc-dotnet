using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SimpleWebAppMVC.Models;
using System.Threading.Tasks;

namespace SimpleWebAppMVC.Controllers
{
    // ValidateAntiForgeryToken: http://go.microsoft.com/fwlink/?LinkId=317598

    public class AccountController(SignInManager<IdentityUser> sm, UserManager<IdentityUser> um) : Controller
    {
        private readonly SignInManager<IdentityUser> signInManager = sm;
        private readonly UserManager<IdentityUser>   userManager   = um;

        // GET /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST /Account/Login
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    var result = await this.signInManager.PasswordSignInAsync(user, model.Password, isPersistent: model.RememberMe, false);

                    if (result.Succeeded)
                        return LocalRedirect(model.ReturnUrl ?? "/");
                }
            }

            ModelState.AddModelError("Password", "Invalid username or password.");

            return View(model);
        }

        // GET /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await this.signInManager.SignOutAsync();

            return Redirect("/");
        }

        // GET /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST /Account/Register
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    Email    = model.Email,
                    UserName = model.Email
                };

                var result = await this.userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await this.signInManager.SignInAsync(user, isPersistent: model.RememberMe, authenticationMethod: "Password");

                    return LocalRedirect(model.ReturnUrl ?? "/");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("Password", error.Description);
            }

            return View(model);
        }
    }
}
