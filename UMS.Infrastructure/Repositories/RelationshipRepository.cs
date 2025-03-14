using UMS.Application.Interfaces.Repositories;
using UMS.Domain.ValueObjects;
using UMS.Persistence.Context;

namespace UMS.Infrastructure.Repositories;

public class RelationshipRepository : BaseRepository<UserRelationship>, IRelationshipRepository
{
    public RelationshipRepository(AppDbContext dbContext) : base(dbContext) { }
}
