using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.PortableExecutable;

namespace environmentMonitor.Data
{
    public class DataSourceClient
    {
        private readonly EnvironmentContext context;

        public DataSourceClient(EnvironmentContext context)
        {
            this.context = context;
        }

        public async Task<long> GetOrCreateAsync(string machineId, string key)
        {
            var try1 = await context.DataSources.Where(e => e.MachineId == machineId && e.Key == key).FirstOrDefaultAsync();

            if (try1 != null)
            {
                return try1.Id;
            }

            var ds = (new DataSource(0, machineId, key));
            await context.DataSources.AddAsync(ds);
            await context.SaveChangesAsync();
            return ds.Id;
        }

        public async Task<List<DataSource>> GetAllAsync()
        {
            var result = await context.DataSources.ToListAsync();

            return result;
        }
    }
}
