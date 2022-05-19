using Microsoft.AspNetCore.Identity;

namespace Cloth.Models
{
    public class AppUser:IdentityUser
    {
        public string? FirstName { get; set; }
    }
}
