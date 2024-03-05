using MedicineMarketPlace.BuildingBlocks.Email.Models;
using MedicineMarketPlace.BuildingBlocks.Email.Services;
using MedicineMarketPlace.BuildingBlocks.EntityFramework;
using MedicineMarketPlace.BuildingBlocks.Firebase.Models;
using MedicineMarketPlace.BuildingBlocks.Firebase.Services;
using MedicineMarketPlace.BuildingBlocks.Identity.Constants;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.BuildingBlocks.Identity.Services.Jwt;
using MedicineMarketPlace.BuildingBlocks.Redis.Services;
using MedicineMarketPlace.BuildingBlocks.Sms.Models;
using MedicineMarketPlace.BuildingBlocks.Sms.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace MedicineMarketPlace.BuildingBlocks
{
    public static class ServiceCollectionExtensions
    {
        private const string JWT_SECTION = "Jwt";
        private const string SmsConfiguration = "Sms";
        private const string FirebaseConfiguration = "Firebase";
        private const string EmailConfigSection = "Email";
        private const string MongoDbSection = "MongoDb";


        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfiguration = configuration.GetSection(JWT_SECTION).Get<JwtConfiguration>();
            services.AddSingleton(jwtConfiguration);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidAudience = jwtConfiguration.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Key))
                };
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(ApplicationUserPolicies.AdminPolicy, policy => policy.RequireRole(ApplicationUserRoles.Admin));
                opt.AddPolicy(ApplicationUserPolicies.SchoolAdminPolicy, policy => policy.RequireRole(ApplicationUserRoles.SchoolAdmin));
                opt.AddPolicy(ApplicationUserPolicies.TeacherPolicy, policy => policy.RequireRole(ApplicationUserRoles.Teacher));
                opt.AddPolicy(ApplicationUserPolicies.StudentPolicy, policy => policy.RequireRole(ApplicationUserRoles.Student));
                opt.AddPolicy(ApplicationUserPolicies.MultiPolicy, policy => policy.RequireRole(ApplicationUserRoles.Admin, ApplicationUserRoles.SchoolAdmin, ApplicationUserRoles.Teacher, ApplicationUserRoles.Student));
                opt.AddPolicy(ApplicationUserPolicies.UserPolicy, policy => policy.RequireRole(ApplicationUserRoles.User));
                opt.AddPolicy(ApplicationUserPolicies.AdminUserPolicy, policy => policy.RequireRole(ApplicationUserRoles.Admin, ApplicationUserRoles.User));
            });

            services.AddScoped<IJwtService, JwtService>();
            return services;
        }

        public static IServiceCollection AddDbHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHealthChecks()
                .AddDbHealthCheck(configuration);

            return services;
        }

        public static IServiceCollection AddSmsServices(this IServiceCollection services, IConfiguration configuration)
        {
            var smsConfiguration = configuration.GetSection(SmsConfiguration).Get<SmsConfiguration>();
            services.AddSingleton(smsConfiguration);

            services.AddScoped<ISmsService, SmsService>();
            return services;
        }

        public static IServiceCollection AddFirebaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            var firebaseConfiguration = configuration.GetSection(FirebaseConfiguration).Get<FirebaseConfiguration>();
            services.AddSingleton(firebaseConfiguration);

            services.AddScoped<IFirebaseService, FirebaseService>();
            return services;
        }

        public static IServiceCollection AddEmailServices(this IServiceCollection services, IConfiguration configuration)
        {
            var emailConfiguration = configuration.GetSection(EmailConfigSection).Get<EmailConfiguration>();
            services.AddSingleton(emailConfiguration);

            services.AddScoped<IEmailService, EmailService>();
            return services;
        }

        public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(c => {
                var redisConfiguration = ConfigurationOptions.Parse(configuration
                    .GetConnectionString("RedisConnection"), true);
                return ConnectionMultiplexer.Connect(redisConfiguration);
            });

            services.AddScoped<IResponseCacheService, ResponseCacheService>();
            return services;
        }


    }
}
