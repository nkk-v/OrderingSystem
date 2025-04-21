using Microsoft.AspNetCore.Identity;

namespace OrderingSystem.Data
{
    public static class DbSeeder
    {
        public static async Task IdentitySeeder(RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new[] { "Admin", "Customer" };

            foreach (var role in roleNames) 
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
