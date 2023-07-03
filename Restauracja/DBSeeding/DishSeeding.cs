using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restauracja.Data;
using Restauracja.Models;
using System.ComponentModel.DataAnnotations;

namespace Restauracja.DBSeeding
{
    public class DishSeeding : Controller
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new RestauracjaContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<RestauracjaContext>>()))
            {
                // Look for any movies.
                if (context.Dish.Any())
                {
                    return;   // DB has been seeded
                }

                context.Dish.AddRange(
                    new Dish
                    {
                        Name = "Zupa jakaś taka",
                        Description = "Bardzo fajna zupka",
                        CreationDate = DateTime.Now,
                        Price = 25,
                        IsAvaliable = true,
                        Category = context.Category.Find(1)
                    },
                    new Dish
                    {
                        Name = "Schabowy i to smaczny",
                        Description = "Oj smaczny schabowy",
                        CreationDate = DateTime.Now,
                        Price = 30,
                        IsAvaliable = true,
                        Category = context.Category.Find(1)
                    },
                    new Dish
                    {
                        Name = "Amen Ramen",
                        Description = "Atadakimas",
                        CreationDate = DateTime.Now,
                        Price = 35,
                        IsAvaliable = true,
                        Category = context.Category.Find(1)
                    },
                    new Dish
                    {
                        Name = "Kompocik",
                        Description = "Picie dobre takie ",
                        CreationDate = DateTime.Now,
                        Price = 5,
                        IsAvaliable = true,
                        Category = context.Category.Find(1)
                    }
                );
                context.SaveChanges();
            }
        } 
    }
}
