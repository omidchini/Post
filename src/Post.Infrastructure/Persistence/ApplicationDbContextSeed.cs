using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using Post.Domain.Entities;
using Post.Domain.ValueObjects;
using Post.Infrastructure.Identity;

namespace Post.Infrastructure.Persistence {
    public static class ApplicationDbContextSeed {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) {
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name)) {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser {
                UserName = "administrator@localhost",
                Email = "administrator@localhost"
            };

            if (userManager.Users.All(u => u.UserName != administrator.UserName)) {
                await userManager.CreateAsync(administrator, "Administrator1!");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context) {
            // Seed, if necessary
            if (!context.Zones.Any()) {
                context.Zones.Add(new Zone {
                    Title = "Zone1",
                    Color = Color.Blue,
                    Items = {
                        new Delivery {
                            Title = "Item 1",
                            Done = true
                        },
                        new Delivery {
                            Title = "Item 2",
                            Done = true
                        },
                        new Delivery {
                            Title = "Item 3",
                            Done = true
                        },
                        new Delivery { Title = "Item 4" },
                        new Delivery { Title = "Item 5" }
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}