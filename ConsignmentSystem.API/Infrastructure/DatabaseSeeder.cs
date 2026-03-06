using ConsignmentSystem.Application.Common.Constants;
using ConsignmentSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace ConsignmentSystem.API.Infrastructure
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDefaultUserAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            string[] roles = { AppRoles.Admin, AppRoles.Storekeeper, AppRoles.Accountant };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role, Id = Guid.NewGuid() });
                }
            }

            await CreateUserAsync(userManager, "admin@consignment.com", "Admin User", AppRoles.Admin);
            await CreateUserAsync(userManager, "store@consignment.com", "Store Keeper", AppRoles.Storekeeper);
            await CreateUserAsync(userManager, "finance@consignment.com", "Accountant User", AppRoles.Accountant);
        }

        private static async Task CreateUserAsync(UserManager<ApplicationUser> userManager, string email, string fullName, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    IsActive = true
                };

                // كلمة مرور موحدة للاختبار
                await userManager.CreateAsync(user, "System@12345");
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
