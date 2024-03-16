using MedicineMarketPlace.BuildingBlocks.EntityFramework.Context;
using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using MedicineMarketPlace.Shared.Entities;
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

            builder.Entity<TaxStatus>(_ =>
            {
                _.ToTable(TaxStatus.TaxStatusTable);
                _.HasKey(_ => _.Id);
                _.HasIndex(_ => _.Name).IsUnique();
            });
        }
    }
}
