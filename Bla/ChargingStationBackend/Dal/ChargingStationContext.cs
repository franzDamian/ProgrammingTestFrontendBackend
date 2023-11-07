using Dal.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class ChargingStationContext : DbContext
    {
        public ChargingStationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ChargingStation> ChargingStations { get; set; }
        
        public DbSet<SimulationInput> SimulationInputs { get; set; }
        
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
        }
    }
}
