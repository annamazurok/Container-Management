using Application.Common.Interfaces.Queries;
using Domain.Entities;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services)
    {
        var connectionString  = "Server=localhost;Port=5432;Database=container-db;User Id=postgres;Password=4321;";
        
        var dataSourseBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourseBuilder.EnableDynamicJson();
        var dataSource = dataSourseBuilder.Build();
        
        services.AddDbContext<ApplicationDbContext>(options => options
            .UseNpgsql(
                dataSource,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            .UseSnakeCaseNamingConvention()
            .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));
        
        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ContainerRepository>();
        services.AddScoped<ContainerTypeRepository>();
        services.AddScoped<UnitRepository>();
        services.AddScoped<HistoryRepository>();
        services.AddScoped<ProductRepository>();
        services.AddScoped<ProductTypeRepository>();
        services.AddScoped<UserRepository>();
        services.AddScoped<ContainerTypeProductTypeRepository>();
        services.AddScoped<RoleRepository>();
        services.AddScoped<IRepository<Container>>(provider => provider.GetRequiredService<ContainerRepository>());
        services.AddScoped<IRepository<ContainerType>>(provider => provider.GetRequiredService<ContainerTypeRepository>());
        services.AddScoped<IRepository<Unit>>(provider => provider.GetRequiredService<UnitRepository>());
        services.AddScoped<IRepository<History>>(provider => provider.GetRequiredService<HistoryRepository>());
        services.AddScoped<IRepository<Product>>(provider => provider.GetRequiredService<ProductRepository>());
        services.AddScoped<IRepository<ProductType>>(provider => provider.GetRequiredService<ProductTypeRepository>());
        services.AddScoped<IContainerTypeProductTypeRepository>(provider => provider.GetRequiredService<ContainerTypeProductTypeRepository>());
        services.AddScoped<IContainerTypeProductTypeQuery>(provider => provider.GetRequiredService<ContainerTypeProductTypeRepository>());
        services.AddScoped<IRepository<User>>(provider => provider.GetRequiredService<UserRepository>());
        services.AddScoped<IRepository<Role>>(provider => provider.GetRequiredService<RoleRepository>());
        services.AddScoped<IContainerQueries>(provider => provider.GetRequiredService<ContainerRepository>());
        services.AddScoped<IContainerTypeQueries>(provider => provider.GetRequiredService<ContainerTypeRepository>());
        services.AddScoped<IUnitQueries>(provider => provider.GetRequiredService<UnitRepository>());
        services.AddScoped<IHistoryQueries>(provider => provider.GetRequiredService<HistoryRepository>());
        services.AddScoped<IProductQueries>(provider => provider.GetRequiredService<ProductRepository>());
        services.AddScoped<IProductTypeQueries>(provider => provider.GetRequiredService<ProductTypeRepository>());
        services.AddScoped<IUserQueries>(provider => provider.GetRequiredService<UserRepository>());
        services.AddScoped<IRoleQueries>(provider => provider.GetRequiredService<RoleRepository>());
        
    }
}