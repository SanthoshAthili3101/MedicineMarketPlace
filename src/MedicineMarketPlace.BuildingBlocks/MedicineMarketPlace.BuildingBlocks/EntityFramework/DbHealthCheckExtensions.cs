using MedicineMarketPlace.BuildingBlocks.EntityFramework.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineMarketPlace.BuildingBlocks.EntityFramework
{
    public static class DbHealthCheckExtensions
    {
        private const string DbHealthChecksSection = "DbHealthChecks";

        public static IHealthChecksBuilder AddDbHealthCheck(this IHealthChecksBuilder healthChecksBuilder, IConfiguration configuration)
        {
            var healthChecksConfiguration = configuration.GetSection(DbHealthChecksSection).Get<DbHealthChecksConfiguration>();

            if (healthChecksConfiguration?.CheckDatabaseHealth == true)
            {
                string databaseConnectionString = configuration.GetConnectionString("DefaultConnection");

                if (!string.IsNullOrEmpty(databaseConnectionString))
                {
                    healthChecksBuilder.AddSqlServer(configuration.GetConnectionString("DefaultConnection"));
                }
            }

            return healthChecksBuilder;
        }
    }
}
