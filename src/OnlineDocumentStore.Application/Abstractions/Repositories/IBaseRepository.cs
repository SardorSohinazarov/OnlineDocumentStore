namespace OnlineDocumentStore.Application.Abstractions.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        ValueTask<TEntity> InsertAsync(TEntity entity);
        ValueTask<TEntity> SelectByIdAsync(long id);
        IQueryable<TEntity> SelectAll();
        ValueTask<TEntity> UpdateAsync(TEntity entity);
        ValueTask<TEntity> DeleteAsync(TEntity entity);
    }
}
