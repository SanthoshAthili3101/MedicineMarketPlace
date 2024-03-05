using MedicineMarketPlace.BuildingBlocks.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicineMarketPlace.BuildingBlocks.EntityFramework.Context
{
    public class EfDbContext : IdentityDbContext<ApplicationUser>
    {
        protected string ConnectionString { get; }
        protected bool EnableEntityFrameworkLogging { get; }

        public EfDbContext(string connectionString, bool enableEntityFrameworkLogging = false)
        {
            ConnectionString = connectionString;
            EnableEntityFrameworkLogging = enableEntityFrameworkLogging;
        }

        public EfDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }
    }
}
