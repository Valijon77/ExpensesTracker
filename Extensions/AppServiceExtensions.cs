using ExpensesTracker.Data;
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
            opts.EnableSensitiveDataLogging(true);
        });
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
