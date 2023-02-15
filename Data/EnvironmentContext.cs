using environmentMonitor.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace environmentMonitor.Data
{
    public class EnvironmentContext : DbContext
    {
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<DataRecord> DataRecords { get; set; }

        public EnvironmentContext(DbContextOptions<EnvironmentContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DataRecord>()
                .HasOne<DataSource>()
                .WithMany()
                .HasForeignKey(p => p.DataSourceId);
        }
    }
}
