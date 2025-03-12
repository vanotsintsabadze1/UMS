using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UMS.Domain.Entities;
using UMS.Domain.ValueObjects;

namespace UMS.Persistence.Context;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }
    public DbSet<UserRelationship> UserRelationships { get; set; }

    private readonly string _connectionString;

    public AppDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}