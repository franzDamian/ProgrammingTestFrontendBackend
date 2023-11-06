using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ChargePointAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChargePointAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChargerController : ControllerBase
    {
        private readonly ILogger<ChargerController> _logger;
        private readonly ChargerContext _context;

        public ChargerController(ILogger<ChargerController> logger, ChargerContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCharger([FromBody] Charger charger)
        {
            // Add charger to database
            _context.Chargers.Add(charger);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharger(int id)
        {
            // Retrieve charger from database
            var charger = await _context.Chargers.FindAsync(id);
            if (charger == null)
            {
                return NotFound();
            }
            return Ok(charger);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCharger(int id, [FromBody] Charger charger)
        {
            // Update charger in database
            if (id != charger.Id)
            {
                return BadRequest();
            }
            _context.Entry(charger).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharger(int id)
        {
            // Delete charger from database
            var charger = await _context.Chargers.FindAsync(id);
            if (charger == null)
            {
                return NotFound();
            }
            _context.Chargers.Remove(charger);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("simulation")]
        public IActionResult RunSimulation([FromBody] SimulationInput input)
        {
            // Run simulation and store results in database
            Random random = new Random();
            var simulationOutput = new SimulationOutput();
            simulationOutput.ChargingValuesPerCharger = new List<double>();
            for (int i = 0; i < input.NumberOfChargers; i++)
            {
                simulationOutput.ChargingValuesPerCharger.Add(random.NextDouble());
            }
            simulationOutput.ExemplaryDay = DateTime.Now;
            simulationOutput.TotalEnergyCharged = random.NextDouble();
            _context.SimulationOutputs.Add(simulationOutput);
            _context.SaveChanges();
            return Ok();
        }
    }

    public class Charger
    {
        public int Id { get; set; }
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