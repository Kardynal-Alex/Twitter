using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Twitter.Domain.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string ProfileImagePath { get; set; }

        public ICollection<TwitterPost> TwitterPosts { get; set; }
    }
}
