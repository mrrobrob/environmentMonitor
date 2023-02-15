using System.Reflection;

namespace environmentMonitor.Data.Models
{
    public class DataRecordFilter
    {
        public static ValueTask<DataRecordFilter> BindAsync(HttpContext context, ParameterInfo parameter)
        {
            if (! DateTime.TryParse(context.Request.Query[nameof(From)], out var from))
            {
                throw new ArgumentNullException(nameof(From));
            }
            
            if (! DateTime.TryParse(context.Request.Query[nameof(To)], out var to))
            {
                {
                    throw new ArgumentNullException(nameof(To));
                }
            }
                        
            var result = new DataRecordFilter
            {
                From = from,
                To = to
            };
            return ValueTask.FromResult(result);
        }

        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
    }
}
