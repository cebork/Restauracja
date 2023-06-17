using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Restauracja.Models;

namespace Restauracja.Data
{
    public class RestauracjaContext : DbContext
    {
        public RestauracjaContext (DbContextOptions<RestauracjaContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Dish>? Dish { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<DishIngredient> DishIngredient { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dish>()
                .HasMany(e => e.Ingredients)
                .WithMany(e => e.Dishes)
                .UsingEntity<DishIngredient>(
                    l => l.HasOne<Ingredient>().WithMany().HasForeignKey(e => e.IngredientID),
                    r => r.HasOne<Dish>().WithMany().HasForeignKey(e => e.DishID)
                );
        }
    }
}
