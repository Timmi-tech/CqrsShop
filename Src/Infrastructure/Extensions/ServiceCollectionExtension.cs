using System.Reflection;
using System.Text;
using Application.Interfaces;
using Application.Interfaces.Contracts;
using Application.Interfaces.Services.Contracts;
using Application.Services;
using Domain.Entities.ConfigurationsModels;
using Domain.Entities.Models;
using DotNetEnv;
using Infrastructure.LoggerService;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtension
    {

        // This method configures Serilog as the logging provider for the application.
        public static IHostBuilder ConfigureSerilogService(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });
        }
        // This method registers the ILoggerManager service with the dependency injection container.
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        // This method configures the Entity Framework Core DbContext to use PostgreSQL with a connection string from the configuration.
        public static void ConfigurePostGressContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryWriteDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            IdentityBuilder builder = services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryWriteDbContext>()
            .AddDefaultTokenProviders();
        }
        // This method registers the IServiceManager service with the dependency injection container.
        public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();

        // This method configures JWT authentication for the application using settings from the configuration.
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            Env.Load();

            JwtConfiguration jwtConfiguration = new();
            configuration.Bind(jwtConfiguration.Section, jwtConfiguration);
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");


            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(Options =>
            {
                Options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfiguration.ValidIssuer,
                    ValidAudience = jwtConfiguration.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))

                };
            });
        }
        // This method adds JWT configuration settings to the service collection.
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<JwtConfiguration>(configuration.GetSection("JwtSettings"));

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            return services;
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Cqrs-Shop", Version = "v1" });
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\n\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Authorization",
                            In = ParameterLocation.Header
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }
        // This method registers the IRepositoryManager service with the dependency injection container.
        public static void ConfigureRepositoryManager(this IServiceCollection services) => services.AddScoped<IRepositoryManager, RepositoryManager>();
    }

}