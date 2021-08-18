using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Twitter.Domain.Entities;

namespace Twitter.Persistence.Configurations
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string adminPassword = "admin123";
            string adminName = "Oleksandr";
            string adminSurname = "Kardinal";
            string userEmail = "irenakardynal@gmail.com";
            string userPassword = "ira1234";
            string userName = "Irena";
            string userSurname = "Kardinal";
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    Name = adminName,
                    Surname = adminSurname,
                    Role = "admin"
                };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                User user = new User
                {
                    Email = userEmail,
                    UserName = userEmail,
                    Name = userName,
                    Surname = userSurname,
                    Role = "user",
                    EmailConfirmed = true
                };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}
