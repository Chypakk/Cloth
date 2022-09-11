using Cloth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Cloth.Infrastructure
{

    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleTagHelper : TagHelper
    {

        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public RoleTagHelper(UserManager<AppUser> usrmgr, RoleManager<IdentityRole> rolemgr)
        {
            userManager = usrmgr;
            roleManager = rolemgr;
        }
        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            IdentityRole role = await roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var users in userManager.Users.ToList())
                {
                    if (users != null && await userManager.IsInRoleAsync(users, role.Name))
                    {
                        names.Add(users.UserName);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "Нет пользователей" : string.Join(", ", names));
        }
    }

}

