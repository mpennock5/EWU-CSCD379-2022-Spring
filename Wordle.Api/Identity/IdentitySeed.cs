using Microsoft.AspNetCore.Identity;
using Wordle.Api.Data;

namespace Wordle.Api.Identity
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            await SeedRolesAsync(roleManager);

            // Seed Admin User
            await SeedAdminUserAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
        }

        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            // Seed Admin User, role is admin, must be over 21 and have MasterOfTheUniverse to use all site features
            if (await userManager.FindByNameAsync("Admin@intellitect.com") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "Admin@intellitect.com",
                    Email = "Admin@intellitect.com",
                    DateOfBirth = "1990-01-01",
                    MasterOfTheUniverse = true,
                };


                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd123").Result;

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }
            }
        }

        private static async Task SeedOver21UserAsync(UserManager<AppUser> userManager)
        {
            //Seed user who is over 21 
            if (await userManager.FindByNameAsync("over21user@intellitect.com") == null)
            {
                AppUser user = new AppUser
                {
                    UserName = "over21user@intellitect.com",
                    Email = "over21user@intellitect.com",
                    DateOfBirth = "1990-01-01",
                };

                await userManager.CreateAsync(user, "P@ssw0rd456");
            }
        }
    }
}
