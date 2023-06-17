using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.Net.NetworkInformation;

namespace Restauracja.DBSeeding
{
    public static class RoleSeeding
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RestauracjaContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RestauracjaContext>>()))
            {
                // Look for any movies.
                if (context.Role.Any())
                {
                    return;   // DB has been seeded
                }

                context.Role.AddRange(
                    new Role
                    {
                        Name = "User"
                    },

                    new Role
                    {
                        Name = "Admin"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
