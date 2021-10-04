using EagleRockHub.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub.Extensions
{
    public static class ServicesConfigurationExtension
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EagleRockHub", Version = "v1" });
                c.AddSecurityDefinition(ApiHeaderConstants.ApiKeyHeader, new OpenApiSecurityScheme
                {
                    Description = "Api key needed to access the endpoints. X-Api-Key: My_API_Key",
                    In = ParameterLocation.Header,
                    Name = ApiHeaderConstants.ApiKeyHeader,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = ApiHeaderConstants.ApiKeyHeader,
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = ApiHeaderConstants.ApiKeyHeader
                            },
                         },
                         Array.Empty<string>()
                     }
                });

            });
        }

        public static void AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            //Add redis cache services
            services.AddStackExchangeRedisCache(options =>
            {
                var redisOptions = configuration.GetSection(RedisOptions.RedisConfig).Get<RedisOptions>();
                options.Configuration = $"{redisOptions.HostName}:{redisOptions.Port}";
            });
        }

    }
}
