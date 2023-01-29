using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace environmentMonitor.Data
{
    public class EnvironmentContext : DbContext
    {
        public DbSet<DataRecord> DataRecords { get; set; }

        public EnvironmentContext(DbContextOptions<EnvironmentContext> options)
            : base(options)
        {

        }
    }
}
