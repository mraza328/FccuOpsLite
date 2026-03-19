using FccuOpsLite.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace FccuOpsLite.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles =
            {
                "Admin",
                "LoanOfficer",
                "Processor",
                "Viewer",
                "Member"
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var bootstrapEnabledRaw = configuration["BootstrapAdmin:Enabled"];
            var bootstrapEnabled = bool.TryParse(bootstrapEnabledRaw, out var enabled) && enabled;

            if (!bootstrapEnabled)
            {
                return;
            }

            var adminEmail = configuration["BootstrapAdmin:Email"];
            var adminPassword = configuration["BootstrapAdmin:Password"];
            var adminDisplayName = configuration["BootstrapAdmin:DisplayName"] ?? "System Admin";

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new InvalidOperationException(
                    "Bootstrap admin is enabled, but BootstrapAdmin:Email or BootstrapAdmin:Password is missing.");
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    DisplayName = adminDisplayName,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    throw new InvalidOperationException($"Admin user creation failed: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}