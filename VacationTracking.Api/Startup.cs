using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using VacationTracking.Api.Middleware;
using VacationTracking.Api.PipelineBehaviors;
using VacationTracking.Data;
using VacationTracking.Data.Repository;
using VacationTracking.Data.UnitOfWork;
using VacationTracking.Domain.Commands.Holiday;
using VacationTracking.Domain.Commands.LeaveType;
using VacationTracking.Domain.Models;
using VacationTracking.Service.Queries.Team;
using VacationTracking.Service.Validation.Commands.Holiday;
using VacationTracking.Service.Validation.Commands.LeaveType;
using VacationTracking.Service.Validation.Queries.Team;

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
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add DIs

            services.AddDbContext<VacationTrackingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyConnection")));
            services.AddScoped<DbContext, VacationTrackingContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IRepository<Team>, Repository<Team>>();
            services.AddScoped<IRepository<Company>, Repository<Company>>();
            services.AddScoped<IRepository<Holiday>, Repository<Holiday>>();
            services.AddScoped<IRepository<HolidayTeam>, Repository<HolidayTeam>>();
            services.AddScoped<IRepository<LeaveType>, Repository<LeaveType>>();
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<Vacation>, Repository<Vacation>>();
            services.AddScoped<IRepository<Setting>, Repository<Setting>>();
            services.AddScoped<IRepository<CompanySetting>, Repository<CompanySetting>>();
            services.AddScoped<IRepository<UserSetting>, Repository<UserSetting>>();

            services.AddMediatR(typeof(GetTeamHandler));
            services.AddAutoMapper(typeof(Service.Mapper.AutoMapping));

            // Register the Swagger generator
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // resolve the IApiVersionDescriptionProvider service
                // note: that we have to build a temporary service provider here because one has not been created yet

                // add a swagger document for each discovered API version
                // note: you might choose to skip or document deprecated API versions differently
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
                {
                    Title = "VacationTracker Api",
                    Version = "1.0"
                });

                //c.EnableAnnotations();

                //c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Flow = "implicit",
                //    Description = "OAuth2 Implicit Grant",
                //    AuthorizationUrl = $"{Configuration["AzureAD:Instance"]}{Configuration["AzureAD:TenantId"]}/oauth2/authorize",
                //    Scopes = new Dictionary<string, string>
                //    {
                //        { "read", "Access read operations" },
                //        { "user_impersonation", "vacation-swagger-app" }
                //    },
                //    TokenUrl = $"{Configuration["AzureAD:Instance"]}{Configuration["AzureAD:TenantId"]}/oauth2/authorize",
                //    Type = "oauth2"
                //});

                //c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                //{
                //    { "oauth2", new string[] { } }
                //});

                //c.DocumentFilter<LowercaseDocumentFilter>();
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


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

            services.AddValidatorsFromAssembly(typeof(GetTeamQueryValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviors<,>));
            services.AddTransient(typeof(IValidator<CreateLeaveTypeCommand>), typeof(LeaveTypeCommandValidator));
            services.AddTransient(typeof(IValidator<UpdateLeaveTypeCommand>), typeof(LeaveTypeCommandValidator));
            services.AddTransient(typeof(IValidator<CreateHolidayCommand>), typeof(HolidayCommandValidator));
            services.AddTransient(typeof(IValidator<UpdateHolidayCommand>), typeof(HolidayCommandValidator));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                // build a swagger endpoint for each discovered API version
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
                //c.OAuthClientId(Configuration["AzureAD:ClientId"]);
                //c.OAuthAppName("hydra-swagger-app");
                //c.OAuthAdditionalQueryStringParams(new Dictionary<string, string>()
                //{
                //    { "resource", Configuration["AzureAD:ClientId"] }
                //});
                //c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });

            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseMiddleware<CustomExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
