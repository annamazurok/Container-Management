using Api.Filters;
using Api.Services.Abstract;
using Api.Services.Implementation;
using Application.Common.Interfaces.Services;
using Application.Settings;
using Infrastructure.Persistence.Services;

namespace Api.Modules;
using FluentValidation;

public static class SetupModule
{
    public static void SetupServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });
        services.AddCors();
        services.AddRequestValidation();
        services.AddApplicationSettings(configuration);
        services.AddControllerServices();
    }

    private static void AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
            options.AddPolicy("AllowFrontend", policy =>
                policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            ));
    }

    private static void AddRequestValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<Program>();
    }

    private static void AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        var settings =  configuration.Get<ApplicationSettings>();
        if (settings != null)
        {
            services.AddSingleton(settings);
        }
    }
    
    private static void AddControllerServices(this IServiceCollection services)
    {
        services.AddScoped<IContainerControllerService, ContainerControllerService>();
        services.AddScoped<IContainerTypeControllerService, ContainerTypeControllerService>();
        services.AddScoped<IHistoryControllerService, HistoryControllerService>();
        services.AddScoped<IUnitControllerService, UnitControllerService>();
        services.AddScoped<IProductControllerService, ProductControllerService>();
        services.AddScoped<IProductTypeControllerService, ProductTypeControllerService>();
        services.AddScoped<IUserControllerService, UserControllerService>();
        services.AddScoped<IRoleControllerService, RoleControllerService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IEmailService, EmailService>();
    }
}
