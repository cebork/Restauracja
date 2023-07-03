using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restauracja.Data;
using Restauracja.DBSeeding;
using Restauracja.Models;
using Restauracja.Services;

namespace Restauracja
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<RestauracjaContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("RestauracjaContext") ?? throw new InvalidOperationException("Connection string 'RestauracjaContext' not found.")));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(option =>
            {
                option.IOTimeout = TimeSpan.FromMinutes(60);
            });

            builder.Services.AddScoped<IDishService, DishService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                CategorySeeding.Initialize(services);
                IngredientSeeding.Initialize(services);
                UserRoleSeeding.Initialize(services);
                DishSeeding.Initialize(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }




    }
}