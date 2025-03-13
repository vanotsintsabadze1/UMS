using Microsoft.EntityFrameworkCore.Storage;
using UMS.Application.Interfaces.UOW;
using UMS.Persistence.Context;

namespace UMS.Infrastructure.UOW;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly AppDbContext _dbContext;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void Dispose()
    {
        if (_transaction is not null)
            _transaction.Dispose();
        _dbContext.Dispose();
    }

    public async Task BeginTransaction(CancellationToken cancellationToken)
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task RollbackTransaction(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
            await _dbContext.Database.RollbackTransactionAsync(cancellationToken);
    }

    public async Task CommitTransaction(CancellationToken cancellationToken)
    {
        if (_transaction is not null)
            await _dbContext.Database.CommitTransactionAsync(cancellationToken);
    }
}