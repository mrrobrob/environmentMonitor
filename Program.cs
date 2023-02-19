using environmentMonitor.Data;
using environmentMonitor.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DataSourceClient>();
builder.Services.AddScoped<DataRecordClient>();
builder.Services.AddScoped<EnvironmentContext>();

var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Path.Combine(Environment.GetFolderPath(folder), "environmentMonitor");
Directory.CreateDirectory(path);
var dbPath = Path.Combine(path, "dataRecords.db");

builder.Services.AddDbContext<EnvironmentContext>(x => x.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}


try
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<EnvironmentContext>();

    var pendingMigrationCount = dbContext.Database.GetPendingMigrations().Count();

    if (pendingMigrationCount > 0)
    {
        Console.WriteLine($"Attempting to apply {pendingMigrationCount} migrations...");

        dbContext.Database.Migrate();

        Console.WriteLine("Database has been updated");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}

app.UseStaticFiles();

app.MapGet("/api/dataSource/getAll", ([FromServices] DataSourceClient client) => client.GetAllAsync());
app.MapGet("/api/dataRecord/getAll", (DataRecordFilter filter, [FromServices] DataRecordClient client) => client.GetAllAsync(filter));
app.MapGet("/api/dataRecord/latest", ([FromServices] DataRecordClient client) => client.GetLatestAsync());

app.MapGet("/api/dataRecord/saveTest", ([FromServices] DataRecordClient client) => client.SaveTestAsync());
app.MapPost("/api/dataRecord/save", ([FromBody] UploadData data, [FromServices] DataRecordClient client) => client.SaveAsync(data));

// legacy
app.MapPost("/api/save", ([FromBody] UploadOldDataRecord dataRecord, [FromServices] DataRecordClient client) => client.SaveOldAsync(dataRecord));

app.MapFallbackToFile("index.html");

app.Run();
