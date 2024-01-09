using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Data;
using System.Diagnostics;
using V59Z7I_SOF_2023241.Data;
using V59Z7I_SOF_2023241.Models;
using V59Z7I_SOF_2023241.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace V59Z7I_SOF_2023241.Services
{
    public class WebstoreService
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<Product> productRepo;
        private readonly IRepository<Category> categoryRepo;
        private readonly IRepository<Cart> cartRepo;
        private readonly IRepository<CartItem> cartItemRepo;

        public WebstoreService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IRepository<Product> productRepo, IRepository<Category> categoryRepo, IRepository<Cart> cartRepo, IRepository<CartItem> cartItemRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this.productRepo = productRepo;
            this.categoryRepo = categoryRepo;
            this.cartRepo = cartRepo;
            this.cartItemRepo = cartItemRepo;
        }

        public List<Product> ProductReadAll()
        {
            return productRepo.ReadAll().ToList();
        }

        public List<Product> GetCategoriesWithProducts()
        {
            var products = productRepo.ReadAll();
            var categories = products.Include(p => p.Category).Distinct().ToList();
            return categories;
        }

        public SelectList GetCategoriesForFilter()
        {
            var categories = GetCategoriesWithProducts();
            return new SelectList(categories.Select(x => x.Category.Name).Distinct());
        }


        public List<Product> FilterByCategory(string categoryId)
        {
            var allProducts = ProductReadAll();
            var categories = GetCategoriesWithProducts();
            var products = string.IsNullOrEmpty(categoryId)
                    ? allProducts
                    : allProducts.Where(p => p.Category.Name == categoryId);
            return products.ToList();
        }

        public void AddToCart(string productId, ClaimsPrincipal principal)
        {
            var product = productRepo.ReadAll().Where(x => x.Id == productId).FirstOrDefault();
            var cart = GetOrCreateCart(principal).Result;
            var existingCartItem = cart.CartItems.FirstOrDefault(item => item.Product.Id == productId);
            if (existingCartItem != null)
            {
                existingCartItem.Quantity++;
            }
            else
            {
                var newCartItem = new CartItem
                {
                    Product = product,
                    Quantity = 1,
                    CartId = cart.Id,
                    Cart = cart,
                };
                cart.CartItems.Add(newCartItem);

            }
            cartRepo.Update(cart);
        }
        private async Task<Cart> GetOrCreateCart(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }
            var carts = cartRepo.ReadAll()
                                    .Include(c => c.CartItems)
                                    .ThenInclude(ci => ci.Product);
            var cart = carts.Where(x => x.UserId == user.Id).FirstOrDefault();

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = user.Id
                };

                cartRepo.Create(cart);
            }
            return cart;
        }
        public Cart ViewCart(ClaimsPrincipal principal)
        {
            var userId = _userManager.GetUserId(principal);
            var cart = cartRepo.ReadAll().Include(c => c.CartItems)
                                        .ThenInclude(ci => ci.Product)
                                        .Where(x => x.UserId == userId)
                                        .FirstOrDefault();

            return cart;
        }

        public List<Category> GetCategories()
        {
            return categoryRepo.ReadAll().ToList();
        }


        public void CreateProductOnUI(Product product, IFormFile picturedata)
        {
            using (var stream = picturedata.OpenReadStream())
            {
                byte[] buffer = new byte[picturedata.Length];
                stream.Read(buffer, 0, (int)picturedata.Length);
                string filename = product.Id + "." + picturedata.FileName.Split('.')[1];
                product.ImageFileName = filename;
                product.Data = buffer;
                product.ContentType = picturedata.ContentType;
            }
            var category = categoryRepo.Read(product.CategoryId);
            product.Category = category;
        }

        public void CreateProduct(Product product)
        {
            productRepo.Create(product);
        }

        public Category CategoryRead(string categoryId)
        {
            return categoryRepo.Read(categoryId);
        }

        public void UpdateCategory(Category category)
        {
            categoryRepo.Update(category);
        }

        public void ProductUpdate(Product product)
        {
            productRepo.Update(product);
        }


        public Product GetProductWithCategoryById(string id)
        {
            return productRepo.ReadAll().Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }

        public Product GetProductById(string id)
        {
            return productRepo.Read(id);
        }

        public Cart GetCartByUserId(string id)
        {
            return cartRepo.ReadAll().Single(x => x.UserId == id);
        }



        public void DeleteItemFromCart(Cart cart, string id)
        {
            cartItemRepo.Delete(id);
        }

        public void DeleteCart(string id)
        {
            cartRepo.Delete(id);
        }

        public async Task AddIdentityRole(IdentityUser user)
        {
            var role = new IdentityRole()
            {
                Name = "Admin"
            };
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        public async Task<IEnumerable<CartItem>> GetUserCartItemsByUserId(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            var items = cartRepo.ReadAll().Include(c => c.CartItems)
                                        .ThenInclude(ci => ci.Product)
                                        .Where(x => x.UserId == user.Id)
                                        .Select(x => x.CartItems)
                                        .FirstOrDefault();
            return items;
        }

        public async Task<IdentityUser> GetCurrentUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }


        public IEnumerable<IdentityUser> GetAllUsers()
        {
            return _userManager.Users;
        }

        public async Task RemoveAdmin(string uid)
        {
            var user = _userManager.Users.FirstOrDefault(t => t.Id == uid);
            await _userManager.RemoveFromRoleAsync(user, "Admin");
        }

        public async Task GrantAdmin(string uid)
        {
            var user = _userManager.Users.FirstOrDefault(t => t.Id == uid);
            await _userManager.AddToRoleAsync(user, "Admin");
        }


        public void DeleteProduct(string id)
        {
            productRepo.Delete(id);
        }

    }
}
