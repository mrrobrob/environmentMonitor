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
                To = to,
                Machine = GetQueryValue(context, nameof(Machine)),
                Key = GetQueryValue(context, nameof(Key))
            };

            return ValueTask.FromResult(result);
        }

        private static string GetQueryValue(HttpContext context, string queryStringKey)
        {
            string? result = context.Request.Query[queryStringKey];

            if (result == null)
            {
                throw new NullReferenceException(queryStringKey);
            }

            return result;
        }

        public required string Machine { get; set; }
        public required string Key { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
    }
}
