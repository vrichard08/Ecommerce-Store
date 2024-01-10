using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using V59Z7I_SOF_2023241.Data;
using V59Z7I_SOF_2023241.Models;
using V59Z7I_SOF_2023241.Services;

namespace V59Z7I_SOF_2023241.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender emailSender;
        private readonly WebstoreService webstoreService;

        public HomeController(IEmailSender emailSender, WebstoreService webstoreService)
        {
            this.emailSender = emailSender;
            this.webstoreService = webstoreService;
        }

        public IActionResult Index()
        {
            var selectList = webstoreService.GetCategoriesForFilter();

            ViewBag.Categories = selectList;
            return View(webstoreService.ProductReadAll());
        }

        public ActionResult FilterByCategory(string categoryId)
        {
            var selectList = webstoreService.GetCategoriesForFilter();
            ViewBag.Categories = selectList;
            return View("Index", webstoreService.FilterByCategory(categoryId));
        }

        [Authorize]
        public IActionResult AddToCart(string productId)
        {
            webstoreService.AddToCart(productId, this.User);
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult ViewCart()
        {
            var cart = webstoreService.ViewCart(this.User);

            return View(cart);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var categories = webstoreService.GetCategories();
            ViewBag.CategoryId = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile picturedata)
        {

            webstoreService.CreateProductOnUI(product, picturedata);
            var category = webstoreService.CategoryRead(product.CategoryId);
            product.Category = category;
            ModelState.Remove("Category");
            if (!ModelState.IsValid)
            { return RedirectToAction(nameof(Create), product); }
            webstoreService.CreateProduct(product);
            if (category != null)
            {
                category.Products.Add(product);
                webstoreService.UpdateCategory(category);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(string id)
        {
            var product = webstoreService.GetProductWithCategoryById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult GetImage(string id)
        {
            var product = webstoreService.GetProductById(id);
            if (product.ContentType.Length > 3)
            {
                return new FileContentResult(product.Data, product.ContentType);
            }
            else
            {
                return BadRequest();
            }

        }
        public IActionResult DeleteItemFromCart(Cart cart, string id)
        {
            webstoreService.DeleteItemFromCart(cart, id);
            return RedirectToAction(nameof(ViewCart));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(string productId)
        {
            var product = webstoreService.GetProductById(productId);
            return View("Update", product);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateItem(Product product)
        {
            var oldProduct = webstoreService.GetProductById(product.Id);
            oldProduct.Price = product.Price;
            oldProduct.StockQuantity = product.StockQuantity;
            webstoreService.ProductUpdate(oldProduct);
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Buy()
        {
            var items = await webstoreService.GetUserCartItemsByUserId(this.User);
            string htmlBody = "<table>\n<thead>\r\n<th>Name</th>\r\n<th>Quantity</th>\r\n<th>Price</th>\r\n</thead>";
            int sum = 0;
            foreach (var item in items)
            {
                htmlBody += $"<tr>\n<td>{item.Product.Name}</td>\n<td>{item.Quantity}</td>\n<td>{item.Product.Price}</td>\n</tr>";
                sum += item.Product.Price * item.Quantity;
                if(item.Product.StockQuantity - item.Quantity < 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    item.Product.StockQuantity -= item.Quantity;
                }
                webstoreService.ProductUpdate(item.Product);
            }
            var user = await webstoreService.GetCurrentUser(this.User);
            htmlBody += $"</tr>\n</table><br><br><h4>Total: {sum}</h4>";
            await emailSender.SendEmailAsync(user.Email, "Order Confirmation", htmlBody);
            var cart = webstoreService.GetCartByUserId(user.Id);
            webstoreService.DeleteCart(cart.Id);
            return View();
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DelegateAdmin()
        {
            var user = await webstoreService.GetCurrentUser(this.User);
            await webstoreService.AddIdentityRole(user);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Users()
        {
            var users = webstoreService.GetAllUsers();
            return View(users);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAdmin(string uid)
        {
            await webstoreService.RemoveAdmin(uid);
            return RedirectToAction(nameof(Users));
        }


        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GrantAdmin(string uid)
        {
            await webstoreService.GrantAdmin(uid);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public IActionResult Delete(string id ) { 
            webstoreService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}