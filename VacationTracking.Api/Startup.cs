using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VacationTracking.Data.IRepositories;
using VacationTracking.Data.Repositories;
using VacationTracking.Service.Dxos;
using MediatR;
using Dapper.FluentMap;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Queries.Team;

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
                config.AddMap(new TeamsMap());
                config.AddMap(new UserMap());
                config.AddMap(new TeamMemberMap());
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add DIs
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamDxos, TeamDxos>();
            services.AddScoped<IUserDxos, UserDxos>();

            services.AddMediatR(typeof(GetTeamHandler).Assembly);

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
