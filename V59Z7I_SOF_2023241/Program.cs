using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using V59Z7I_SOF_2023241.Data;
using V59Z7I_SOF_2023241.Models;
using V59Z7I_SOF_2023241.Services;

namespace V59Z7I_SOF_2023241
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); 
                

            // Add services to the container.
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => 
            {
                options.SignIn.RequireConfirmedAccount = true;
                
            }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IRepository<Product>, ProductRepository>();
            builder.Services.AddTransient<IRepository<Category>, CategoryRepository>();
            builder.Services.AddTransient<IRepository<Cart>, CartRepository>();
            builder.Services.AddTransient<IRepository<CartItem>, CartItemRepository>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<WebstoreService>();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer("Data Source=localhost;Initial Catalog=MyWebstore;Integrated Security=True;MultipleActiveResultSets=True"));


            builder.Services.AddAuthentication().AddFacebook(opt =>
            {
                opt.AppId = "1366805410891645";
                opt.AppSecret = "75ce9170ccc46fb51475a3f95495e341";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}