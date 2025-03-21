﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Cloth.Models
{
    public class UserModel
    {
        public class UserViewModel
        {
            [Required]
            public string Login { get; set; }
            [Required]
            public string Email { get; set; }
            [Required]
            public string Password { get; set; }
        }

        public class LoginModel
        {
            [Required]
            [UIHint("login")]
            public string Login { get; set; }
            [Required]
            [UIHint("password")]
            public string Password { get; set; }
        }

        public class RoleEditModel
        {
            public IdentityRole Role { get; set; }
            public IEnumerable<AppUser> Members { get; set; }
            public IEnumerable<AppUser> NonMembers { get; set; }
        }

        public class RoleModificationModel
        {
            [Required]
            public string RoleName { get; set; }
            public string RoleId { get; set; }
            public string[]? IdsToAdd { get; set; }
            public string[]? IdsToDelete { get; set; }
        }
    }
}
