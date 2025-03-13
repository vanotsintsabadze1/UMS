namespace UMS.Application.Interfaces.UOW;

public interface IUnitOfWork
{
    Task BeginTransaction(CancellationToken cancellationToken);
    Task RollbackTransaction(CancellationToken cancellationToken);
    Task CommitTransaction(CancellationToken cancellationToken);
}