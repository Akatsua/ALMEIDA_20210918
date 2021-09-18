using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gateway.Application;
using Infrastructure;
using Gateway.Infrastructure;
using Gateway.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                {
                    builder.AllowAnyOrigin();
                });
            });

            services.AddDbContext<CategoryContext>(
                options => options.UseNpgsql(Configuration.GetConnectionString("CategoryContext")));

            services.AddTransient<CategoryRepository>();

            services
                .AddSingleton<ArchiveRepository>()
                .Configure<ArchiveRepositoryConfiguration>(Configuration.GetSection("Archive"));

            services
                .AddSingleton<FileUtils>()
                .Configure<FileUtilsConfiguration>(Configuration.GetSection("FileValidation"));

            services
                .AddSingleton<AzuriteRepository>()
                .Configure<AzuriteConfiguration>(Configuration.GetSection("Azurite"));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gateway", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gateway v1"));
            }

            //app.UseHttpsRedirection();
            app.UseCors("CORS");
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
