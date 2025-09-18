using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace todoapi.API.service;

public static class JwtAuthBuilderExtensions
{
    public static AuthenticationBuilder AddJwtAuthentication(this IServiceCollection services, JwtConfiguration jwtConfiguration)
    {
        services.AddAuthorization();
        Console.WriteLine(" I have arrived");
        return services.AddAuthentication(JwtOptions =>
        {
            JwtOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            JwtOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret)),
                RequireExpirationTime = true,
            };
        });
    }
}