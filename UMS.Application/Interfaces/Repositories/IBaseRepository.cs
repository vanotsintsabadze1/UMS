using System.Linq.Expressions;

namespace UMS.Application.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
}