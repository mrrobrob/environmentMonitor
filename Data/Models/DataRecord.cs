using Microsoft.EntityFrameworkCore;

namespace environmentMonitor.Data.Models
{
    [Index(nameof(When))]
    public record class DataRecord(long Id, DateTime When, long DataSourceId, double Value);
}