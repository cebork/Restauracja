using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.Net.NetworkInformation;

namespace Restauracja.DBSeeding
{
    public static class CategorySeeding
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RestauracjaContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RestauracjaContext>>()))
            {
                // Look for any movies.
                if (context.Category.Any())
                {
                    return;   // DB has been seeded
                }

                context.Category.AddRange(
                    new Category
                    {
                        Name = "Ostre"
                    },

                    new Category
                    {
                        Name = "Troche mniej ostre"
                    },

                    new Category
                    {
                        Name = "Troche bardziej ostre"
                    },

                    new Category
                    {
                        Name = "Takie O"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
