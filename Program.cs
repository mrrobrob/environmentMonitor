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

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/getAll", async ([FromServices]DataClient client) =>
{
    return await client.GetAll();
});
app.MapPost("/api/save", async ([FromBody]UploadDataRecord dataRecord, [FromServices] DataClient client) => {
    await client.Save(dataRecord);
});



app.Run();
