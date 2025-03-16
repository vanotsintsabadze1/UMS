using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UMS.Persistence.Context;

namespace UMS.Persistence;

public static class DbMigrator
{
    public static async Task Migrate(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dmsDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dmsDbContext.Database.MigrateAsync();
    }
}   