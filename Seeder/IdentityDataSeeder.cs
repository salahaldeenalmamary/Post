namespace Comment_Post.Seeder
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Comment_Post.Constant;
    using Comment_Post.Models;
    using Microsoft.AspNetCore.Identity;

   

    public static class IdentityDataSeeder
    {
        public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            await CreateDefaultUserAsync(userManager, "admin@3.com", "Admin@123", "Admin");
            await CreateDefaultUserAsync(userManager, "user@example.com", "User@123","User");
          
        }

        private static async Task CreateDefaultUserAsync(UserManager<AppUser> userManager, string email, string password, string role)
        {
            var existingUser = await userManager.FindByEmailAsync(email);

            if (existingUser == null)
            {
                var newUser = new AppUser { UserName = email, Email = email, CreatedAt
                    = DateTime.Now };

                var result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, role);
                }
                else
                {
                    throw new Exception($"Failed to create default user '{email}'.");
                }
            }
        }
    }


}
