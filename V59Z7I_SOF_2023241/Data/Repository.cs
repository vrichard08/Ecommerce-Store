using Microsoft.EntityFrameworkCore.Migrations;

namespace V59Z7I_SOF_2023241.Data
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext ctx;

        public Repository(ApplicationDbContext ctx)
        {
            this.ctx = ctx;
        }

        public void Create(T item)
        {
            ctx.Set<T>().Add(item);
            ctx.SaveChanges();
        }

        public IQueryable<T> ReadAll()
        {
            return ctx.Set<T>();
        }

        public void Delete(string id)
        {
            ctx.Set<T>().Remove(Read(id));
            ctx.SaveChanges();
        }

        public abstract T Read(string id);
        public abstract void Update(T item);
    }
}
