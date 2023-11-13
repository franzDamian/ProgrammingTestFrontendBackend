using System.Runtime.InteropServices;
using ChargingStationBackend.SimulationCalculation;
using Dal.Model;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public static void Test1()
        {
            // create a few simulation charging stations
            // create a simulation service
            // simulate a year
            // check if the values are correct

            var testSim = new SimulationService();
            var testList = new List<SimChargingStation>()
            {
                // create a few simulation charging stations
                new()
                {
                    ChargingPower = 11
                },
                new()
                {
                    ChargingPower = 5
                },
                new()
                {
                    ChargingPower = 22
                },
                new()
                {
                    ChargingPower = 11
                },
                new()
                {
                    ChargingPower = 5
                },
                new()
                {
                    ChargingPower = 22
                }
            };
            var testInput = new SimulationInput()
            {
                AverageConsumptionOfCars = 15,
                ArrivalProbabilityMultiplier = 1,
                ChargingStations = testList.Select(cs => new ChargingStation()
                {
                    ChargingPower = cs.ChargingPower
                }).ToList()
            };

            var testOutput = testSim.SimulationRun(testInput);
            // check if the values are correct
            Assert.InRange(testOutput.TotalEnergyCharged, 0, 1000000);
            Assert.InRange(testOutput.NumberOfChargingEventsPerYear, 0, 1000000);
            Assert.InRange(testOutput.NumberOfChargingEventsPerMonth, 0, 1000000);
        }
    }
}