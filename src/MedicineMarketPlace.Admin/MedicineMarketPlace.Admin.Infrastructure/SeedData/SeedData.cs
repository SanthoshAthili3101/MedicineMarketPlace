using MedicineMarketPlace.BuildingBlocks.Identity.Constants;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineMarketPlace.Admin.Infrastructure.SeedData
{
    public static class SeedData
    {
        public static async Task SeedUsersAndRoles(this IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<IdentityRole>
            {
                new IdentityRole {Name= ApplicationUserRoles.Admin },
                new IdentityRole {Name= ApplicationUserRoles.SuperAdmin }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new ApplicationUser
            {
                Email = "super.admin@phibonacci.com",
                UserName = "super.admin@phibonacci.com"
            };

            await userManager.CreateAsync(admin, "$Team@123");
            await userManager.AddToRoleAsync(admin, ApplicationUserRoles.SuperAdmin);
        }
    }
}
