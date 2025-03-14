using UMS.Application.Interfaces.Repositories;
using UMS.Domain.Entities;
using UMS.Persistence.Context;

namespace UMS.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext dbContext) : base(dbContext) { }
}