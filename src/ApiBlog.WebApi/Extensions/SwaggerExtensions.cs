using Microsoft.OpenApi.Models;

namespace ApiBlog.WebApi.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Posts API",
                Description = "Minimal API, that allows to create, edit and publish text based posts"
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Bearer scheme token to send in request Authorization header",
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    []
                }
            });

            c.EnableAnnotations();
        });

        return services;
    }
}