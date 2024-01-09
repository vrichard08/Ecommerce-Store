using V59Z7I_SOF_2023241.Models;

namespace V59Z7I_SOF_2023241.Data
{
    public class CategoryRepository : Repository<Category>, IRepository<Category>
    {
        public CategoryRepository(ApplicationDbContext ctx) : base(ctx)
        {
        }

        public override Category Read(string id)
        {
            return ctx.Categories.FirstOrDefault(p => p.Id == id);
        }

        public override void Update(Category item)
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
