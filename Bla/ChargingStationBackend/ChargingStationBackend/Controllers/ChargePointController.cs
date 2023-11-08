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
    public record ChargerDto(int ChargingPower);

    public record SimulationInputDto(
        List<ChargingStation> ChargingStations,
        int AverageConsumptionOfCars,
        int ArrivalProbabilityMultiplier);

    public record SimulationOutputDto(
        List<List<double>> ChargingValuesPerChargingStationPerDay,
        int TotalEnergyCharged,
        int NumberOfChargingEventsPerYear,
        int NumberOfChargingEventsPerMonth,
        int NumberOfChargingEventsPerWeek,
        int NumberOfChargingEventsPerDay,
        double DeviationOfConcurrencyFactor);


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
            _context.ChargingStations.Add(new ChargingStation { ChargingPower = chargerDto.ChargingPower });

            await _context.SaveChangesAsync();
        }

        [HttpPost("addSimulation")]
        public async Task PostSimulationInputAsync([FromBody] SimulationInputDto simulationInputDto)
        {
            _context.SimulationInputs.Add(new SimulationInput());
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<ActionResult<List<ChargingStation>>> GetChargerAsync()
        {
            var chargingStations = await _context.ChargingStations.ToListAsync();
            return chargingStations;
        }

        [HttpPost("addOutput")]
        public async Task PostSimulationOutputAsync([FromBody] SimulationOutputDto simulationOutputDto)
        {
            _context.SimulationOutputs.Add(new SimulationOutput
            {
                ChargingValuesPerChargingStationPerDay = simulationOutputDto.ChargingValuesPerChargingStationPerDay,
                TotalEnergyCharged = simulationOutputDto.TotalEnergyCharged,
                NumberOfChargingEventsPerYear = simulationOutputDto.NumberOfChargingEventsPerYear,
                NumberOfChargingEventsPerMonth = simulationOutputDto.NumberOfChargingEventsPerMonth,
                NumberOfChargingEventsPerWeek = simulationOutputDto.NumberOfChargingEventsPerWeek,
                NumberOfChargingEventsPerDay = simulationOutputDto.NumberOfChargingEventsPerDay,
                DeviationOfConcurrencyFactor = simulationOutputDto.DeviationOfConcurrencyFactor
            });
            await _context.SaveChangesAsync();
        }

        [HttpGet("getOutput")]
        public async Task<ActionResult<List<SimulationOutput>>> GetOutPutAsync()
        {
            var output = await _context.SimulationOutputs.ToListAsync();
            return output;
        }
    }
}