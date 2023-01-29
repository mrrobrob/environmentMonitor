namespace environmentMonitor.Data
{
    public record class DataRecord(long Id, DateTime When, string MachineId, string Key, double Value);
    public record class UploadDataRecord(string MachineId, string Key, double Value);
}