using UMS.Domain.Entities;

namespace UMS.Application.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<ICollection<User>> GetUserByQueryLike(string query, int page, int pageSize, CancellationToken cancellationToken);
}
