using Microsoft.Extensions.Configuration;

namespace MedicineMarketPlace.BuildingBlocks.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetConfiguration<T>(this IConfiguration configuration, string configSectionName)
        {
            var configSection = configuration.GetSection(configSectionName).Get<T>();
            if (configSection == null)
            {
                throw new Exception($"No configuration section found with name: {configSectionName}");
            }

            return configSection;
        }

        // Replace later with desired string
        public static string GetPhiTeacherTrainingIdentityConnectionString(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("PhiTeacherTrainingIdentityConnection");
        }
    }
}
