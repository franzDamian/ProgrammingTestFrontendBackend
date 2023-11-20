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
                .Property(chargingStation => chargingStation.ChargingPower).IsRequired();

            modelBuilder.Entity<SimulationInput>()
                .Property(simulationInput => simulationInput.AverageConsumptionOfCars).IsRequired();


            modelBuilder.Entity<SimulationOutput>()
                .Property(simulationOutput => simulationOutput.TotalEnergyCharged).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(simulationOutput => simulationOutput.NumberOfChargingEventsPerYear).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(simulationOutput => simulationOutput.NumberOfChargingEventsPerMonth).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(simulationOutput => simulationOutput.NumberOfChargingEventsPerWeek).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(simulationOutput => simulationOutput.NumberOfChargingEventsPerDay).IsRequired();
            modelBuilder.Entity<SimulationOutput>()
                .Property(simulationOutput => simulationOutput.DeviationOfConcurrencyFactor).IsRequired();
            modelBuilder
                .Entity<SimulationOutput>()
                .Property(simulationOutput => simulationOutput.ChargingValuesPerChargingStationPerDay)
                .HasConversion(x => JsonConvert.SerializeObject(x),
                    y => JsonConvert.DeserializeObject<List<List<double>>>(y) ?? new());
        }
    }
}