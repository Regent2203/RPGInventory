using Microsoft.EntityFrameworkCore;
using RPGInventory.Application;
using RPGInventory.Infrastructure;
using RPGInventory.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        //sqlite tip: to ensure db is actually created on disk
        await context.Database.OpenConnectionAsync();
        await context.Database.MigrateAsync();

        await ApplicationDbContextSeed.SeedInitialDataAsync(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error while creating or migrating DB.");
    }
}

app.Run();