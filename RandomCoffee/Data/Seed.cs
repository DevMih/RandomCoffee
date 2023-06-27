using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RandomCoffee.Entities;

namespace RandomCoffee.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var users = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "test1",
                    FirstName = "Jake",
                    SecondName = "White",
                    Email = "test@test"

                },
                new AppUser
                {
                    UserName = "test2",
                    FirstName = "Lisa",
                    SecondName = "Brown",
                    Email = "test@test"
                }
            };

            var roles = new List<AppRole>
            {
                new AppRole
                {
                    Name = "Member"
                },
                new AppRole
                {
                    Name = "Admin"
                }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin",
                FirstName = "Admin",
                SecondName = "Admin",
                Email = "test@test"
            };

            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}
