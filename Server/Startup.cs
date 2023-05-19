using Application.IRepository;
using Application.Mapper;
using Application.Services;
using FluentValidation;
using Infrastructure.Database;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Server.Models;
using System;
using System.Reflection;

namespace Server
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
            AddDatabase(services);
            
            services.AddAutoMapper(Assembly.GetAssembly(typeof(AutoMapperProfile)));

            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IChartRepository, ChartRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IDepthChartService, DepthChartService>();

            services.AddScoped<IValidator<PlayerRequest>, PlayerValidator>();
            services.AddScoped<IValidator<ChartRequest>, ChartRequestValidator>();
            services.AddScoped<IValidator<ChartRemoveRequest>, ChartRemoveValidator>();
            

            services.AddControllers()
                .AddNewtonsoftJson(opt =>
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Server", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Server v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddDatabase(IServiceCollection services)
        {
            services.AddOptions();
            var databaseSetting = Configuration.GetSection("DatabaseSetting");
            services.Configure<DatabaseSetting>(databaseSetting);
            var connectionString = DatabaseConnection.GetConnectionString(databaseSetting.Get<DatabaseSetting>());
            services.AddPooledDbContextFactory<AppDbContext>(opt => {
                Console.WriteLine(connectionString);
                opt.UseSqlServer(connectionString);
            });

            //services.AddDbContext<AppDbContext>();
            services.AddScoped<AppDbContext>(x =>
                x.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext()
            );
        }
    }
}
