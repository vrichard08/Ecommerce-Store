using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using V59Z7I_SOF_2023241.Models;

namespace V59Z7I_SOF_2023241.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public  DbSet<Product> Products { get; set; }
        public  DbSet<Cart> Carts { get; set; }
        public  DbSet<CartItem> CartItems { get; set; }
        public  DbSet<Category> Categories { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            string path = Path.GetFullPath("~/images/programming_basics.jpg");

            builder.Entity<Product>(product => product
                        .HasOne(product => product.Category)
                        .WithMany(category => category.Products)
                        .HasForeignKey(product => product.CategoryId)
                        .OnDelete(DeleteBehavior.Cascade));// look up

            builder.Entity<CartItem>( item => item
                        .HasOne(item => item.Cart)
                        .WithMany(cart => cart.CartItems)
                        .HasForeignKey(item => item.CartId)
                        .OnDelete(DeleteBehavior.Cascade));

            builder.Entity<Product>().HasData(
                new Product { Id = "36184642-c410-409e-9a38-f78b14fcc2fd", Name = "Programming Bacics", Description = "Book version of Programming Basics", Price = 40, StockQuantity = 6, CategoryId = "C8ACB820-2A12-43A9-8A42-52657D1D22A4", Data = System.IO.File.ReadAllBytes(Path.GetFullPath("wwwroot/images/programming_basics.jpg")), ContentType = "image/jpeg" },
                new Product { Id = "4c5d9051-8f78-409f-866f-44516926ec93", Name = "Iphone", Description = "Iphone 15 Pro", Price = 1000, StockQuantity = 8, CategoryId = "8AD52F22-C53F-4B8A-B5E8-0A39095412F1", Data = System.IO.File.ReadAllBytes(Path.GetFullPath("wwwroot/images/iphone.jpg")), ContentType = "image/jpeg" },
                new Product { Id = "661cce3c-77d8-4de0-a29f-0cb50912e9cd", Name = "Sweater", Description = "Warm wool sweater", Price = 20, StockQuantity = 20, CategoryId = "327C91D3-5299-479C-841C-D194864CEB35", Data = System.IO.File.ReadAllBytes(Path.GetFullPath("wwwroot/images/sweater.jpg")), ContentType = "image/jpeg" },
                new Product { Id = "c1b29f3c-7483-41a1-8917-689516d53437", Name = "The Great Gatsby", Description = "The book version of The Great Gatsby original version", Price = 20, StockQuantity = 39, CategoryId = "C8ACB820-2A12-43A9-8A42-52657D1D22A4", Data = System.IO.File.ReadAllBytes(Path.GetFullPath("wwwroot/images/great_gatsby.jpg")), ContentType = "image/jpeg" },
                new Product { Id = "f18d83a2-21ad-45ec-bb4f-a2e4d173bc47", Name = "Jordan 1 Chicago", Description = "Limited sneaker", Price = 1000, StockQuantity = 2, CategoryId = "327C91D3-5299-479C-841C-D194864CEB35", Data = System.IO.File.ReadAllBytes(Path.GetFullPath("wwwroot/images/shoes.jpg")), ContentType = "image/jpeg" }
                );
            builder.Entity<Category>().HasData(
                new Category { Id = "08536157-CE10-4AFA-B550-406114A0DD57", Name = "Home Appliances" },
                new Category { Id = "327C91D3-5299-479C-841C-D194864CEB35", Name = "Clothing" },
                new Category { Id = "8AD52F22-C53F-4B8A-B5E8-0A39095412F1", Name = "Electronics" },
                new Category { Id = "C8ACB820-2A12-43A9-8A42-52657D1D22A4", Name = "Books" },
                new Category { Id = "D4EAB1AB-EC06-48A4-9A14-4B1DDFFBB67C", Name = "Sports Equipment" });
            base.OnModelCreating(builder);
            

        }

        
    }
}