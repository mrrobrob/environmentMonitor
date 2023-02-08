using environmentMonitor.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DataClient>();
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

{
    var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<EnvironmentContext>();
    dbContext.Database.Migrate();
}

app.UseStaticFiles();

app.MapGet("/api/getAll", ([FromServices] DataClient client) => client.GetAll());
app.MapGet("/api/latest", ([FromServices] DataClient client) => client.GetLatest());
app.MapPost("/api/save", ([FromBody] UploadDataRecord dataRecord, [FromServices] DataClient client) => client.Save(dataRecord));

app.MapGet("/api/saveTest", ([FromServices] DataClient client) => client.SaveTest());

app.MapFallbackToFile("index.html");

app.Run();
