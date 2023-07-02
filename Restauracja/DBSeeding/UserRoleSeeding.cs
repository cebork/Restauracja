using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.Net.NetworkInformation;

namespace Restauracja.DBSeeding
{
    public static class UserRoleSeeding
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RestauracjaContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RestauracjaContext>>()))
            {
                // Look for any movies.
                if (!context.Role.Any())
                {
                    context.Role.AddRange(
                        new Role
                        {
                            Name = "User"
                        },

                        new Role
                        {
                            Name = "Admin"
                        }
                    );   // DB has been seeded
                }

                
                context.SaveChanges();
                if (!context.User.Any())
                {
                    var passwordHasher = new PasswordHasher<User>();

                    context.User.AddRange(
                        new User
                        {
                            Email = "Admin@Admin.Admin",
                            FirstName = "Admin",
                            LastName = "Admin",
                            City = "City",
                            PostalCode = "21-400",
                            Address = "Address",
                            PhoneNumber = "000000000",
                            PasswordHash = passwordHasher.HashPassword(null, "admin"),
                            ActivationCode = "Admin123",
                            RoleId = 2,
                            IsActive = true

                        },
                        new User
                        {
                            Email = "User@User.User",
                            FirstName = "User",
                            LastName = "User",
                            City = "User",
                            PostalCode = "21-400",
                            Address = "User",
                            PhoneNumber = "000000001",
                            PasswordHash = passwordHasher.HashPassword(null, "user"),
                            ActivationCode = "User123",
                            RoleId = 1,
                            IsActive = true
                        }
                    );
                }
                context.SaveChanges();
            }
        }
    }
}
