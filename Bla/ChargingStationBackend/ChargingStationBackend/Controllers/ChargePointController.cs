using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace ChargePointAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChargerController : ControllerBase
    {
        private readonly ILogger<ChargerController> _logger;
        private readonly IMongoDatabase _database;

        public ChargerController(ILogger<ChargerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateCharger([FromBody] Charger charger)
        {
            // Add charger to database
            _database.GetCollection<Charger>("chargers").InsertOne(charger);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetCharger(int id)
        {
            // Retrieve charger from database
            var charger = _database.GetCollection<Charger>("chargers").Find(x => x.Id == id).FirstOrDefault();
            return Ok(charger);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCharger(int id, [FromBody] Charger charger)
        {
            // Update charger in database
            _database.GetCollection<Charger>("chargers").ReplaceOne(x => x.Id == id, charger);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCharger(int id)
        {
            // Delete charger from database
            _database.GetCollection<Charger>("chargers").DeleteOne(x => x.Id == id);
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
            _database.GetCollection<SimulationOutput>("simulations").InsertOne(simulationOutput);
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
