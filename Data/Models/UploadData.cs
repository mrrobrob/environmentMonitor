namespace environmentMonitor.Data.Models
{
    public record class UploadData(string MachineId, Dictionary<string, double> Records);


}