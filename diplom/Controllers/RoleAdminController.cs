using Cloth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Cloth.Models.UserModel;

namespace Cloth.Controllers
{
    public class RoleAdminController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;

        public RoleAdminController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMrg)
        {
            this.roleManager = roleMgr;
            this.userManager = userMrg;
        }

        public IActionResult AdminPage() => View(roleManager.Roles);
        public IActionResult UserView() => View(userManager.Users);

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded)
            {
                return RedirectToAction("AdminPage");
            }
            else
            {
                AddErrorsFromResult(result);
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("AdminPage");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Роль не найдена");
            }
            return View("AdminPage", roleManager.Roles);
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonmembers = new List<AppUser>();
            foreach (AppUser user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonmembers;
                list.Add(user);
            }
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleModificationModel model, string UN)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    AppUser user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                }
                foreach (string UserId in model.IdsToDelete ?? new string[] { })
                {

                    AppUser usera = await userManager.FindByNameAsync(UN);
                    AppUser user = await userManager.FindByIdAsync(UserId);

                    if (user != null && usera != null)
                    {
                        if (user != usera)
                        {
                            result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                            if (!result.Succeeded)
                            {
                                AddErrorsFromResult(result);
                            }
                        }
                        
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(AdminPage));
            }
            else
            {
                return await Edit(model.RoleId);
            }
        }


    }
}
