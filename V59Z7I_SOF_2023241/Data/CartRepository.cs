using V59Z7I_SOF_2023241.Models;

namespace V59Z7I_SOF_2023241.Data
{
    public class CartRepository : Repository<Cart>, IRepository<Cart>
    {
        public CartRepository(ApplicationDbContext ctx) : base(ctx)
        {
        }

        public override Cart Read(string id)
        {
            return ctx.Carts.FirstOrDefault(p => p.Id == id);
        }

        public override void Update(Cart item)
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
