using System.Text;
using ExpensesTracker.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ExpensesTracker.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings); // Alt: configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings)

        services
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme) // I: returns 'AuthenticationBuilder'
            .AddJwtBearer(options =>
            {
                // O: check how 'ValidateLifetime' property works. I: It makes sure if expiration date is in the future.
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.TokenKey) // [O]: Use Options pattern
                    ),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        return services;
    }
}
