using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.Net.NetworkInformation;

namespace Restauracja.DBSeeding
{
    public static class IngredientSeeding
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RestauracjaContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RestauracjaContext>>()))
            {
                // Look for any movies.
                if (context.Ingredient.Any())
                {
                    return;   // DB has been seeded
                }

                context.Ingredient.AddRange(
                    new Ingredient
                    {
                        Name = "Sól"
                    },

                    new Ingredient
                    {
                        Name = "Pieprz"
                    },

                    new Ingredient
                    {
                        Name = "Halapeno"
                    },

                    new Ingredient
                    {
                        Name = "Woda"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
