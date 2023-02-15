using Microsoft.EntityFrameworkCore;

namespace environmentMonitor.Data.Models
{
    [Index(nameof(MachineId), nameof(Key))]
    public record class DataSource(long Id, string MachineId, string Key);


}