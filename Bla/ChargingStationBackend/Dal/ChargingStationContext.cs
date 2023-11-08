using Dal.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dal
{
    public class ChargingStationContext : DbContext
    {
        public ChargingStationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ChargingStation> ChargingStations { get; set; }
        
        public DbSet<SimulationInput> SimulationInputs { get; set; }

        public DbSet<SimulationOutput> SimulationOutputs { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChargingStation>()
                .Property(ChargingStation => ChargingStation.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ChargingStation>()
                .Property(cs => cs.ChargingPower).IsRequired();
            
            modelBuilder.Entity<SimulationInput>()
                .Property(SimulationInput => SimulationInput.id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SimulationInput>()
                .Property(SimulationInput => SimulationInput.AverageConsumptionOfCars).IsRequired();

            modelBuilder.Entity<SimulationOutput>()
                .Property(SimulationOutput => SimulationOutput.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<SimulationOutput>()
                .Property(SimulationOutput => SimulationOutput.TotalEnergyCharged).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(SimulationOutput => SimulationOutput.NumberOfChargingEventsPerYear).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(SimulationOutput => SimulationOutput.NumberOfChargingEventsPerMonth).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(SimulationOutput => SimulationOutput.NumberOfChargingEventsPerWeek).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(SimulationOutput => SimulationOutput.NumberOfChargingEventsPerDay).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(SimulationOutput => SimulationOutput.DeviationOfConcurrencyFactor).IsRequired();
            // Generate the charging values per charging station per day in the database with a primary key for each list<double> in the list
            modelBuilder.Entity<SimulationOutput>().HasKey(SimulationOutput => SimulationOutput.ChargingValuesPerChargingStationPerDay);
            modelBuilder
                .Entity<SimulationOutput>()
                .Property(SimulationOutput => SimulationOutput.ChargingValuesPerChargingStationPerDay)
                .HasConversion(x => JsonConvert.SerializeObject(x), y => JsonConvert.DeserializeObject<List<List<double>>>(y) ?? new ());

        }
    }
}
