using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            base.OnModelCreating(builder);

        }
    }
}