using Microsoft.EntityFrameworkCore;

namespace BulkyBookWeb.EFCore.Repository
{
    public class EFRepository<TEntity,TContext>:IEFRepository<TEntity>
        where TEntity : class
        where TContext : DbContext
    {
        private readonly TContext _context;

        public EFRepository(TContext context)
        {
            this._context = context;
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> AddAll(List<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TEntity> Delete(int id)
        {
            var obj = await _context.Set<TEntity>().FindAsync(id);
            if (obj != null)
            {
                _context.Set<TEntity>().Remove(obj);
                await _context.SaveChangesAsync();
                return obj;
            }
            else
            {
                return null;
            }
        }

        public async Task<TEntity> Get(int id)
        {
            var obj= await _context.Set<TEntity>().FindAsync(id);
            if (obj != null)
            {
                return obj;
            }
            else
                return null;
        }

        public async Task<List<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
             _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;

        }
    }
}
