namespace BulkyBookWeb.EFCore.Repository
{
    public interface IEFRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<T> Update(T entity);
        Task<T> Delete(int id);
        Task<T> Add (T entity);
        Task<bool> AddAll(List<T> entities);
        Task<List<T>> GetAll();

    }
}
