using environmentMonitor.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace environmentMonitor.Data
{
    public class DataRecordClient
    {
        private readonly EnvironmentContext context;
        private readonly DataSourceClient dataSourceClient;

        public DataRecordClient(EnvironmentContext context, DataSourceClient dataSourceClient)
        {
            this.context = context;
            this.dataSourceClient = dataSourceClient;
        }

        public async Task<List<DataRecord>> GetAllAsync(DataRecordFilter filter)
        {
            var data = await context.Set<DataRecord>()
                .Where(e => e.When > filter.From && e.When < filter.To)
                .OrderByDescending(e => e.When)
                .ToListAsync();

            return data;
        }

        public async Task<List<DataRecord>> GetLatestAsync()
        {
            var data = await context.Set<DataRecord>()
                .GroupBy(e => e.DataSourceId)
                .Select(e => e.OrderByDescending(e => e.When).First())
                .ToListAsync();
            return data;
        }

        public async Task SaveOldAsync([FromBody] UploadOldDataRecord dataRecord)
        {
            var dataSourceId = await dataSourceClient.GetOrCreateAsync(dataRecord.MachineId, dataRecord.Key);
            await context.DataRecords.AddAsync(new DataRecord(0, DateTime.UtcNow, dataSourceId, dataRecord.Value));
            await context.SaveChangesAsync();
        }

        public Task SaveTestAsync()
        {
            return SaveAsync(new UploadData("Dummy Machine", new Dictionary<string, double>()
            {
                { "Key", 12.5 },
                { "Key2", 23.5 }
            }));
        }

        public async Task SaveAsync(UploadData data)
        {
            foreach (var record in data.Records)
            {
                var dataSourceId = await dataSourceClient.GetOrCreateAsync(data.MachineId, record.Key);
                await context.DataRecords.AddAsync(new DataRecord(0, DateTime.UtcNow, dataSourceId, record.Value));
            }

            await context.SaveChangesAsync();
        }
    }
}