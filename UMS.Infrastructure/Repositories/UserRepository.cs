using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UMS.Application.Interfaces.Repositories;
using UMS.Domain.Entities;
using UMS.Persistence.Context;

namespace UMS.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly DbSet<User> _dbSet;

    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<User>();
    }

    public async Task<ICollection<User>> GetUserByQueryLike(string query, int page, int pageSize, CancellationToken cancellationToken)
    {
        var pattern = $"{query}";
        var offset = (page - 1) * pageSize;

        var users = await _dbSet.Where(u =>
                EF.Functions.Like(u.Firstname, pattern)
                || EF.Functions.Like(u.Lastname, pattern)
                || EF.Functions.Like(u.SocialNumber, pattern))
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return users;
    }
        
    public new async Task<User> GetAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        var user = await _dbSet.Where(predicate)
            .Include(u => u.City)
            .Include(u => u.PhoneNumbers)
            .Include(u => u.Relationships)
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }
    
}