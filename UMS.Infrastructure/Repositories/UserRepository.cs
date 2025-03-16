using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UMS.Application.Interfaces.Repositories;
using UMS.Application.Models.User;
using UMS.Domain.Entities;
using UMS.Infrastructure.Extensions;
using UMS.Persistence.Context;

namespace UMS.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly DbSet<User> _dbSet;

    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
        _dbSet = dbContext.Set<User>();
    }

    public async Task<(ICollection<User> Users, int TotalCount)> GetUserByQueryLike(string query, int page, int pageSize, CancellationToken cancellationToken)
    {
        var pattern = $"%{query}%";
        var offset = (page - 1) * pageSize;

        var usersQuery = _dbSet.Where(u =>
                EF.Functions.Like(u.Firstname + " " + u.Lastname, pattern) // Full name search
                || EF.Functions.Like(u.Firstname, pattern)
                || EF.Functions.Like(u.Lastname, pattern)
                || EF.Functions.Like(u.SocialNumber, pattern))
            .Include(u => u.City)
            .Include(u => u.PhoneNumbers)
            .Include(u => u.RelatedUsers)
            .Include(u => u.RelatedByUsers);
            
        var totalCount = await usersQuery.CountAsync(cancellationToken);
        var users = await usersQuery.ToListAsync(cancellationToken);
        
        return (users, totalCount);
    }

    public async Task<(ICollection<User> Users, int TotalCount)> GetUsersPaginated(UserDetailedSearchRequestModel searchModel, int page, int pageSize, CancellationToken cancellationToken)
    {
        var offset = (page - 1) & pageSize;

        var usersQuery = _dbSet.ApplyFilter(searchModel.Firstname, u => u.Firstname.Contains(searchModel.Firstname))
            .ApplyFilter(searchModel.Lastname, u => u.Lastname.Contains(searchModel.Lastname))
            .ApplyFilter(searchModel.Gender, u => u.Gender == searchModel.Gender)
            .ApplyFilter(searchModel.SocialNumber, u => u.SocialNumber.Contains(searchModel.SocialNumber))
            .ApplyFilter(searchModel.CityId, u => u.CityId == searchModel.CityId)
            .ApplyFilter(searchModel.PhoneNumbers,
                u => u.PhoneNumbers.Any(pn => searchModel.PhoneNumbers.Contains(pn.Number)))
            .Include(u => u.City)
            .Include(u => u.PhoneNumbers)
            .Include(u => u.RelatedUsers)
            .Include(u => u.RelatedByUsers);

        var totalCount = await usersQuery.CountAsync(cancellationToken);
        var users = await usersQuery.ToListAsync(cancellationToken);

        return (users, totalCount);
    }

    public new async Task<User> GetAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken)
    {
        var user = await _dbSet.Where(predicate)
            .Include(u => u.City)
            .Include(u => u.PhoneNumbers)
            .Include(u => u.RelatedUsers)
            .Include(u => u.RelatedByUsers)
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }
}