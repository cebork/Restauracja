using System;
using System.Collections.Generic;
using System.Linq;
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

        public DbSet<Restauracja.Models.Category> Category { get; set; } = default!;

        public DbSet<Restauracja.Models.Dish>? Dish { get; set; }
    }
}
