using ExpensesTracker.Data;
using ExpensesTracker.Helpers;
using ExpensesTracker.Interfaces;
using ExpensesTracker.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpensesTracker.Extensions;

public static class AppServiceExtensions
{
    public static IServiceCollection AddAppServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<DataContext>(opts =>
        {
            opts.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            opts.EnableSensitiveDataLogging(true); // I: For only developement phase of an application
        });
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<ITokenService, TokenService>();
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName)); // I: adding the Options pattern

        return services;
    }
}
