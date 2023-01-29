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
            var data = await context.Set<DataRecord>().ToListAsync();
            return data;
        }

        public async Task Save([FromBody] UploadDataRecord dataRecord)
        {
            await context.DataRecords.AddAsync(new DataRecord(0, DateTime.UtcNow, dataRecord.MachineId, dataRecord.Key, dataRecord.Value));
            await context.SaveChangesAsync();
        }
    }
}