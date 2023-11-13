using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChargingStationBackend.SimulationCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Model;
using Moq;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using Xunit.Sdk;

namespace ChargingStationBackend.SimulationCalculation.Tests
{
    [TestClass()]
    public class SimulationServiceTests
    {
        [TestMethod()]
        public void SimulationRunTest()
        {
            // create a simulation input
            var simInput = new SimulationInput()
            {
                ChargingStations = new List<ChargingStation>
                {
                    new ChargingStation { ChargingPower = 11, Id = new Random().Next(1400) },
                    new ChargingStation() { ChargingPower = 15, Id = new Random().Next(1400) }
                },
                AverageConsumptionOfCars = 18,
                ArrivalProbabilityMultiplier = 1
            };

            // create a simulation service
            var simulationService = new SimulationService();
            // run the simulation
            var simulationOutput = simulationService.SimulationRun(simInput);
            // check if the simulation output is not null
            Assert.IsNotNull(simulationOutput);
            // check if the simulation output has the correct values
            Assert.AreEqual(2, simulationOutput.TotalEnergyCharged);
            Assert.Fail();
        }

        [TestMethod()]
        public void GetMaxPKTest()
        {
            Assert.Fail();
        }
    }
}