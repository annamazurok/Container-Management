using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) :  DbContext(options)
{
    public DbSet<Container> Containers { get; init; }
    public DbSet<ContainerType> ContainerTypes { get; init; }
    public DbSet<Unit> Units { get; init; }
    public DbSet<History> Histories { get; init; }
    public DbSet<Product> Products { get; init; }
    public DbSet<ProductType> ProductTypes { get; init; }
    public DbSet<User> Users { get; init; }
    public DbSet<Role> Roles { get; init; }
    public DbSet<ContainerTypeProductType> ContainerTypeProductTypes { get; init; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.HasAnnotation("Relational:MigrationHistoryTable", "__efmigrationshistory");
        base.OnModelCreating(modelBuilder);
    }
}