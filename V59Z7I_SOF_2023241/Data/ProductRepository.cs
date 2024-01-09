using V59Z7I_SOF_2023241.Models;

namespace V59Z7I_SOF_2023241.Data
{
    public class ProductRepository : Repository<Product>, IRepository<Product>
    {
        public ProductRepository(ApplicationDbContext ctx) : base(ctx)
        {
                
        }
        public override Product Read(string id)
        {
            return ctx.Products.FirstOrDefault(p => p.Id == id);
        }

        public override void Update(Product item)
        {
            var old = Read(item.Id);
            foreach (var prop in old.GetType().GetProperties())
            {
                if (prop.GetAccessors().FirstOrDefault(t => t.IsVirtual) == null)
                {
                    prop.SetValue(old, prop.GetValue(item));
                }
            }
            ctx.SaveChanges();
        }
    }
}
