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
            context.TwitterPosts.AddRange(InitialData.ExpectedTwitterPosts);

            context.Images.AddRange(InitialData.ExpectedImages);

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
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    ProfileImagePath = "Image path1"
                },
                new User
                {
                    Id = "5ae019a1-c312-4589-ab62-8b8a1fcb882c",
                    Name = "Ira",
                    UserName = "irakardinal@gmail.com",
                    Surname = "Kardynal",
                    Role = "user",
                    Email = "irakardinal@gmail.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("ira123"),
                    ProfileImagePath = "Image path2"
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

            context.Comments.AddRange(InitialData.ExpectedComments);

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
