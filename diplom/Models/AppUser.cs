﻿using Microsoft.AspNetCore.Identity;

namespace Cloth.Models
{
    public class AppUser:IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Adress { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Index { get; set; }
    }
}
