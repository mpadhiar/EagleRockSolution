using EagleRockHub.Extensions;
using EagleRockHub.Interfaces;
using EagleRockHub.Middleware;
using EagleRockHub.Models;
using EagleRockHub.Repositories;
using EagleRockHub.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EagleRockHub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSwagger();
            services.AddRedisCache(Configuration);
            services.Configure<AppSettings>(Configuration.GetSection(nameof(AppSettings)));
            services.AddSingleton<IEagleHubRepository, EagleHubRepository>();
            services.AddSingleton<ICacheProviderService, CacheProviderService>();
            services.AddSingleton<IEagleHubService, EagleHubService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EagleRockHub v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ApiKeyMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
