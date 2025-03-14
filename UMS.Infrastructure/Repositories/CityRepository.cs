using UMS.Application.Interfaces.Repositories;
using UMS.Domain.Entities;
using UMS.Persistence.Context;

namespace UMS.Infrastructure.Repositories;

public class CityRepository : BaseRepository<City>, ICityRepository
{
    public CityRepository(AppDbContext dbContext) : base(dbContext) { }
}
