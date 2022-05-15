using Cloth.Models;
using Cloth.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Cloth.Models.UserModel;

namespace Cloth.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private DataContext Context;

        public AccountController(UserManager<AppUser> userMngr, SignInManager<AppUser> signInMngr, DataContext ctx)
        {
            userManager = userMngr;
            signInManager = signInMngr;
            Context = ctx;
        }

        [AllowAnonymous]
        public IActionResult Login(string? returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(details.Login);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Password), "Неверный пароль или логин");
            }
            return View(details);
        }

        [AllowAnonymous]
        public IActionResult CreateUser() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Login,
                    Email = model.Email
                };
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (IdentityError eror in result.Errors)
                    {
                        ModelState.AddModelError("", eror.Description);
                    }
                }

            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AdminProfile()
        {
            var result = new ProfileViewModel
            {
                Picture = Context.Pictures.Where(a => a.Name == User.Identity.Name),
                CartLines = Context.CartLine.Include(a => a.Product),
                Order = Context.Orders.Include(a => a.Lines).Where(a => a.Name == User.Identity.Name)
            };
            return View(result);
        } 
        public IActionResult Profile()
        {
            var result = new ProfileViewModel
            {
                Picture = Context.Pictures.Where(a => a.Name == User.Identity.Name),
                CartLines = Context.CartLine.Include(a => a.Product),
                Order = Context.Orders.Include(a => a.Lines).Where(a => a.Name == User.Identity.Name)
            };
            return View(result);
        }
    }
}
