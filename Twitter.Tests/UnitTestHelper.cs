using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twitter.Domain.Entities;
using Twitter.Persistence;
using Twitter.Services.Mapping;

namespace Twitter.Tests
{
    internal static class UnitTestHelper
    {
        public static DbContextOptions<ApplicationContext> GetUnitDbOptions()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new ApplicationContext(options))
            {
                SeedData(context);
            }

            return options;
        }

        public static void SeedData(ApplicationContext context)
        {
            context.TwitterPosts.Add(new TwitterPost
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                PostText = "TwitterPost text1",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
            });
            context.TwitterPosts.Add(new TwitterPost
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                PostText = "TwitterPost text2",
                DateCreation = DateTime.Now.Date,
                Like = 3,
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53"
            });
            context.TwitterPosts.Add(new TwitterPost
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                PostText = "TwitterPost text3",
                DateCreation = DateTime.Now.Date,
                Like = 0,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            });
            context.TwitterPosts.Add(new TwitterPost
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                PostText = "TwitterPost text4",
                DateCreation = DateTime.Now.Date,
                Like = 5,
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c"
            });

            context.Images.Add(new Images
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                Image1 = "Resources\\Images\\11-1.jpg",
                Image2 = "Resources\\Images\\11-2.jpg",
                Image3 = "Resources\\Images\\11-3.jpg",
                Image4 = "Resources\\Images\\11-4.jpg"
            });
            context.Images.Add(new Images
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                Image1 = "Resources\\Images\\22-1.jpg",
                Image2 = "Resources\\Images\\22-2.jpg",
                Image3 = "Resources\\Images\\22-3.jpg",
                Image4 = "Resources\\Images\\22-4.jpg",
            });
            context.Images.Add(new Images
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                Image1 = "Resources\\Images\\33-1.jpg",
                Image2 = "Resources\\Images\\33-2.jpg",
                Image3 = "Resources\\Images\\33-3.jpg",
                Image4 = "Resources\\Images\\33-4.jpg"
            });
            context.Images.Add(new Images
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                Image1 = "Resources\\Images\\44-1.jpg",
                Image2 = "Resources\\Images\\44-2.jpg",
                Image3 = "Resources\\Images\\44-3.jpg",
                Image4 = "Resources\\Images\\44-4.jpg",
            });
            //BCrypt.Net.BCrypt.Verify("Pa$$w0rd", passwordHash);
            var users = new List<User>
            {
                new User
                {
                    Id = "925695ec-0e70-4e43-8514-8a0710e11d53",
                    Name = "Oleksandr",
                    UserName = "admin@gmail.com",
                    Surname = "Kardynal",
                    Role = "admin",
                    Email = "admin@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123")
                },
                new User
                {
                    Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    Name = "Ira",
                    UserName = "irakardinal@gmail.com",
                    Surname = "Kardynal",
                    Role = "user",
                    Email = "irakardinal@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("ira123")
                }
            }.AsQueryable();
            context.Roles.Add(new IdentityRole { Id = "105695ec-0e70-4e43-8514-8a0710e11d53", Name = "user" });
            context.Roles.Add(new IdentityRole { Id = "012095ec-0e70-4e43-8514-8a0710e11d53", Name = "admin" });
            context.UserRoles.Add(new IdentityUserRole<string>
            {
                UserId = "925695ec-0e70-4e43-8514-8a0710e11d53",
                RoleId = "105695ec-0e70-4e43-8514-8a0710e11d53"
            });
            context.UserRoles.Add(new IdentityUserRole<string>
            {
                UserId = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                RoleId = "012095ec-0e70-4e43-8514-8a0710e11d53"
            });
            context.Users.AddRange(users);

            context.SaveChanges();
        }

        public static Mapper CreateMapperProfile()
        {
            var myProfile = new AutoMapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }
    }
}
