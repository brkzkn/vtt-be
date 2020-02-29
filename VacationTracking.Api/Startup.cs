using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VacationTracking.Data.IRepositories;
using VacationTracking.Data.Repositories;
using MediatR;
using Dapper.FluentMap;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Queries.Team;
using AutoMapper;
using VacationTracking.Service.Commands.Team;
using VacationTracking.Data;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using Microsoft.AspNetCore.Routing;

namespace VacationTracking.Api
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            // Read configuration and combine appsettings.json and appsettings.env.json by environment of deployment
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            FluentMapper.Initialize(config =>
            {
                config.AddMap(new CompaniesMap());
                config.AddMap(new HolidaysMap());
                config.AddMap(new LeaveTypesMap());
                config.AddMap(new TeamsMap());
                config.AddMap(new TeamMemberMap());
                config.AddMap(new UserMap());
                config.AddMap(new VacationsMap());
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add DIs
            services.AddScoped<IHolidayRepository, HolidayRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            services.AddScoped<IDbConnection>(db => new NpgsqlConnection(
                    Configuration.GetConnectionString("MyConnection")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMediatR(typeof(GetTeamHandler).Assembly);
            services.AddMediatR(typeof(GetTeamListHandler).Assembly);
            services.AddMediatR(typeof(CreateTeamHandler).Assembly);

            services.AddAutoMapper(typeof(Service.Mapper.AutoMapping));


            services.AddMemoryCache();

            // Add Cors
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
