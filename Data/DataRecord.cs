using Microsoft.EntityFrameworkCore;

namespace environmentMonitor.Data
{
    [Index(nameof(MachineId), nameof(Key))]
    public record class DataSource(long Id, string MachineId, string Key);

    [Index(nameof(When))]
    public record class DataRecord(long Id, DateTime When, long DataSourceId, double Value);

    public record class UploadOldDataRecord(string MachineId, string Key, double Value);

    public record class UploadData(string MachineId, Dictionary<string, double> Records);


}