using OnlineDocumentStore.Application.Abstractions.Repositories;

namespace OnlineDocumentStore.Infrastructure.Repositories
{
    public class BaseRepsitory<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;

        public BaseRepsitory(AppDbContext context)
            => _context = context;

        public async ValueTask<TEntity> InsertAsync(TEntity entity)
        {
            var entry = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async ValueTask<TEntity> SelectByIdAsync(long id)
            => await _context.Set<TEntity>().FindAsync(id);

        public IQueryable<TEntity> SelectAll()
            => _context.Set<TEntity>();

        public async ValueTask<TEntity> UpdateAsync(TEntity entity)
        {
            var entry = _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }

        public async ValueTask<TEntity> DeleteAsync(TEntity entity)
        {
            var entry = _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return entry.Entity;
        }
    }
}
