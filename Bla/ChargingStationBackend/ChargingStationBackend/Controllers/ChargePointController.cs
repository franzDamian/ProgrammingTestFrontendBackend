using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using ChargingStationBackend.SimulationCalculation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Dal;
using Dal.Model;

namespace ChargingStationBackend.Controllers
{
    public record ChargerDto(int ChargingPower);

    public record ChargingStationDto(
        int ChargingPower,
        List<List<double>> ChargingValuesForEachDayAndHour
    );

    public record SimulationInputDto(
        List<ChargingStationDto> ChargingStations,
        int AverageConsumptionOfCars,
        int ArrivalProbabilityMultiplier);


    [ApiController]
    [Route("[controller]")]
    public class ChargerController : ControllerBase
    {
        private readonly SimulationService _simulationService;

        private readonly ChargingStationContext _context;

        public ChargerController(ChargingStationContext context, SimulationService simulationService)
        {
            _context = context;
            _simulationService = simulationService;
        }

        [HttpPost("AddChargingStation")]
        public async Task PostChargerAsync([FromBody] ChargerDto chargerDto)
        {
            _context.ChargingStations.Add(new ChargingStation { ChargingPower = chargerDto.ChargingPower });

            await _context.SaveChangesAsync();
        }

        [HttpPost("AddSimulationInput")]
        public async Task PostSimulationInputAsync([FromBody] SimulationInputDto simulationInputDto)
        {
            var simInput = new SimulationInput()
            {
                ArrivalProbabilityMultiplier = simulationInputDto.ArrivalProbabilityMultiplier,
                AverageConsumptionOfCars = simulationInputDto.AverageConsumptionOfCars,
                ChargingStations = simulationInputDto.ChargingStations.Select(cs => new ChargingStation()
                {
                    ChargingPower = cs.ChargingPower
                }).ToList()
            };
            _context.SimulationInputs.Add(simInput);
            var simulationOutput = _simulationService.SimulationRun(simInput);
            _context.SimulationOutputs.Add(simulationOutput);
            // Save the simulationInput and simulationOutput to the database
            await _context.SaveChangesAsync();
        }

        [HttpGet("GetChargingStationList")]
        public async Task<ActionResult<List<ChargingStation>>> GetChargerAsync()
        {
            var chargingStations = await _context.ChargingStations.ToListAsync();
            return chargingStations;
        }


        [HttpGet("getOutput")]
        public async Task<ActionResult<List<SimulationOutput>>> GetOutPutAsync()
        {
            var output = await _context.SimulationOutputs.ToListAsync();
            return output;
        }
    }
}