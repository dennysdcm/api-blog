using System;
using System.Text;
using ApiBlog.Application.Services;
using ApiBlog.Domain.Repositories;
using ApiBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace ApiBlog.Infrastructure;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(sp =>
            new MongoClient(configuration.GetConnectionString("MongoDbConnection")));

        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var database = client.GetDatabase("ApiPostDb");
            return database;
        });
        
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IHashProvider, HashProvider>();

        return services;
    }
    
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<JwtTokenSettings>(configuration.GetSection(nameof(JwtTokenSettings)));
        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();


        var authSettings = configuration.GetSection(nameof(JwtTokenSettings)).Get<JwtTokenSettings>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings.Secret)),
                    ValidIssuer = "https://id.com",
                    ValidAudience = "https://posts.com",
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });

        return services;
    }
}