namespace V59Z7I_SOF_2023241.Data
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> ReadAll();
        T Read(string id);
        void Create(T item);
        void Update(T item);
        void Delete(string id);
    }
}