using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Rift.Models;

public class SeedDevAdmin
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string adminName = "admin";
        string adminEmail = "admin@email.com";
        string adminPassword = "Admin1!";
        string adminRole = "Admin";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var user = new User
            {
                Name = adminName,
                Email = adminEmail,
                UserName = Guid.NewGuid().ToString()
            };
            var createdUser = await userManager.CreateAsync(user, adminPassword);

            if (createdUser != null)
            {
                await userManager.AddToRoleAsync(user, adminRole);
            }
        }
    }
}