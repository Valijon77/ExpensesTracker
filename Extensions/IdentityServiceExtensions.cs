using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace ExpensesTracker.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme) // I: returns 'AuthenticationBuilder'
            .AddJwtBearer(options =>
            {
                // O: check how 'ValidateLifetime' property works. I: It makes sure if expiration date is in the future.
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["TokenKey"]!) // O: Use Options pattern
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        return services;
    }
}
