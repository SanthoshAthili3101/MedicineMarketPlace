using MedicineMarketPlace.Admin.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineMarketPlace.Admin.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection MMPAdminServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IAdminUserService, AdminUserService>();

            return services;
        }
    }
}
