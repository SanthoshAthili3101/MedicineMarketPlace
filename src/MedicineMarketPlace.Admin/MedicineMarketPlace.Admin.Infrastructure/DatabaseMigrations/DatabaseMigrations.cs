using MedicineMarketPlace.Shared.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MedicineMarketPlace.Admin.Infrastructure.DatabaseMigrations
{
    public static class DatabaseMigrations
    {
        public static async Task MigrateAsync(this IServiceProvider services)
        {
            var medicineMarketDbContext = services.GetRequiredService<MedicineMarketDbContext>();
            await medicineMarketDbContext.Database.MigrateAsync();
        }
    }
}
