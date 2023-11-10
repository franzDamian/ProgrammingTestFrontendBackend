using ChargingStationBackend.SimulationCalculation;
using Dal;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//RunSimulation runSimulation = new RunSimulation();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApiDocument();
builder.Services.AddTransient<SimulationService>();
// connect to postgres database
builder.Services.AddDbContext<ChargingStationContext>(options =>
    options.UseNpgsql(@"Server=localhost;Username=postgres;Database=postgres"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("ClientPermissions",
        policy => { policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

// Create the app.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUI();
}

app.UseCors("ClientPermissions");

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var chargingStationContext = scope.ServiceProvider.GetRequiredService<ChargingStationContext>();
    chargingStationContext.Database.Migrate();
}

app.Run();