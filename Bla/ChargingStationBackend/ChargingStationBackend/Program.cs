using ChargingStationBackend.SimulationCalculation;
using Dal;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
RunSimulation runSimulation = new RunSimulation();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApiDocument();
// connect to postgres database
builder.Services.AddDbContext<ChargingStationContext>(options =>
    options.UseNpgsql(@"Server=localhost;Username=postgres;Database=postgres"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var chargingStationContext = scope.ServiceProvider.GetRequiredService<ChargingStationContext>();
    chargingStationContext.Database.Migrate();
}

app.Run();

