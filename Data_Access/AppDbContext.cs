using Data_Access.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ZstdSharp.Unsafe;

namespace Data_Access
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var con = _configuration.GetConnectionString("Database");
                optionsBuilder
                    .UseMySql(con, ServerVersion.AutoDetect(con))
                    .EnableSensitiveDataLogging();
            }
        }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<Building> Buildings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
