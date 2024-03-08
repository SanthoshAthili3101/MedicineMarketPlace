using MedicineMarketPlace.BuildingBlocks.EntityFramework.Context;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicineMarketPlace.Shared.Context
{
    public class MedicineMarketDbContext : EfDbContext
    {
        public MedicineMarketDbContext(string connectionString, bool enableEntityFrameworkLogging)
       : base(connectionString, enableEntityFrameworkLogging)
        {
        }

        public MedicineMarketDbContext(DbContextOptions<MedicineMarketDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<ApplicationUser>();
        }
    }
}
