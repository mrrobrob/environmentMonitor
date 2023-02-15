using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace environmentMonitor.Data
{
    public class DataClient
    {
        private readonly EnvironmentContext context;

        public DataClient(EnvironmentContext context)
        {
            this.context = context;
        }

        public async Task<List<DataRecord>> GetAll()
        {
            var data = await context.Set<DataRecord>()
                .OrderByDescending(e => e.When)
                .ToListAsync();
            return data;
        }

        public async Task Save([FromBody] UploadDataRecord dataRecord)
        {
            var dataSourceId = await GetOrCreateDataSourceAsync(dataRecord.MachineId, dataRecord.Key);
            await context.DataRecords.AddAsync(new DataRecord(0, DateTime.UtcNow, dataSourceId, dataRecord.Value));
            await context.SaveChangesAsync();
        }

        private async Task<long> GetOrCreateDataSourceAsync(string machineId, string key)
        {            
            var result = await context.DataSources.AddAsync(new DataSource(0, machineId, key));
            await context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task SaveTest()
        {
            var dataSourceId = await GetOrCreateDataSourceAsync("Dummy Machine", "Key");
            await context.DataRecords.AddAsync(new DataRecord(0, DateTime.UtcNow, dataSourceId, 12.5));
            await context.SaveChangesAsync();
        }

        public async Task<List<DataRecord>> GetLatest()
        {
            var data = await context.Set<DataRecord>()
                .GroupBy(e => e.DataSourceId)
                .Select(e => e.OrderByDescending(e => e.When).First())
                .ToListAsync();
            return data;
        }
    }
}