using V59Z7I_SOF_2023241.Models;

namespace V59Z7I_SOF_2023241.Data
{
    public class CartItemRepository : Repository<CartItem>, IRepository<CartItem>
    {
        public CartItemRepository(ApplicationDbContext ctx) : base(ctx)
        {
        }

        public override CartItem Read(string id)
        {
            return ctx.CartItems.FirstOrDefault(x => x.Id == id);
        }

        public override void Update(CartItem item)
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
