using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dal;
using Dal.Model;

namespace ChargePointAPI.Controllers
{
    public record ChargerDto (int chargingPower);
    public record SimulationInputDto(List<ChargingStation> chargingStations, int averageConsumptionOfCars);
    [ApiController]
    [Route("[controller]")]
    public class ChargerController : ControllerBase
    {
        private readonly ChargingStationContext _context;

        public ChargerController(ChargingStationContext context)
        {
            _context = context;
        }
        [HttpPost("add")]
        public async Task PostChargerAsync([FromBody] ChargerDto chargerDto)
        {
            _context.ChargingStations.Add(new ChargingStation { ChargingPower = chargerDto.chargingPower });
            
            await _context.SaveChangesAsync();
        }

        [HttpPost("addSimulation")]
        public async Task PostSimulationInputAsync([FromBody] SimulationInputDto simulationInputDto)
        {
            
            _context.Add(simulationInputDto);
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<ActionResult<List<ChargingStation>>> GetChargerAsync()
        {
            var chargingStations = await _context.ChargingStations.ToListAsync();
            return chargingStations;
        }

    }

    public class Charger
    {
        public int Id { get; set; }
        public double ChargersMaxPowerOutput { get; set; }
        public List<ChargingEvent> ChargingEvents { get; set; }
    }

    public class ChargingEvent
    {
        public int Id { get; set; }
        public double EnergyCharged { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class ChargerList
    {
        public int Id { get; set; }
        public List<Charger> Chargers { get; set; }
        public DateTime ExemplaryDay { get; set; }
        public int NumberOfChargers { get; set; }
        public double ArrivalProbabilityMultiplier { get; set; } = 1.0;
        public double CarConsumption { get; set; } = 18.0;
        public double ChargingPowerPerCharger { get; set; } = 11.0;


    }

    public class SimulationInput
    {
        public int NumberOfChargers { get; set; }
        public double ArrivalProbabilityMultiplier { get; set; } = 1.0;
        public double CarConsumption { get; set; } = 18.0;
        public double ChargingPowerPerCharger { get; set; } = 11.0;
    }

    public class SimulationOutput
    {
        public List<double> ChargingValuesPerCharger { get; set; }
        public DateTime ExemplaryDay { get; set; }
        public double TotalEnergyCharged { get; set; }
        public int NumberOfChargingEvents { get; set; }
    }
}