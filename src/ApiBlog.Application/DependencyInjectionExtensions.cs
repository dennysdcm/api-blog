using ApiBlog.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiBlog.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IPostFactory, PostFactory>();


        services.AddScoped<IPostsService, PostService>();
        services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}