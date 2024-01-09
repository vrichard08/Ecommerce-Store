using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using V59Z7I_SOF_2023241.Data;
using V59Z7I_SOF_2023241.Models;

namespace V59Z7I_SOF_2023241.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IRepository<Product> productRepo;
        private readonly IRepository<Category> categoryRepo;

        public ApiController(IRepository<Product> productRepo, IRepository<Category> categoryRepo, IRepository<Cart> cartRepo, IRepository<CartItem> cartItemRepo)
        {
            this.productRepo = productRepo;
            this.categoryRepo = categoryRepo;
        }

        [HttpGet]
        public IEnumerable<Product> GetItems()
        {
            return this.productRepo.ReadAll();

        }

    }
}
