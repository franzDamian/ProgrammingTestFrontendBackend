using System;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Dal;
using Dal.Model;
using Microsoft.EntityFrameworkCore;

namespace ChargingStationBackend.SimulationCalculation
{
    public class Simulation
    {
        private readonly ChargingStationContext _context;
        private SimulationInput _simulationInput = new SimulationInput();

        private readonly double[] _arrivalDistribution =
        {
            0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 2.83, 2.83, 5.66, 5.66, 5.66, 7.55, 7.55, 7.55, 10.38, 4.72,
            4.72, 4.72, 0.94, 0.94
        };

        private readonly double[] _carNeedsCharging = { 34.31, 4.90, 9.80, 11.76, 8.82, 11.76, 10.78, 4.90, 2.94 };
        private readonly int _ticksPerYear = 35040;
        private readonly int _ticksPerMonth = 35040 / 12;
        private readonly int _ticksPerWeek = 35040 / 54;
        private readonly int _ticksPerDay = 35040 / 365;

        public Simulation(ChargingStationContext chargingStationContext, bool random = false)
        {
            _context = chargingStationContext;
        }

        public async Task<SimulationOutput> SimulationRun()
        {
            if (_context.SimulationInputs is null)
            {
                await Task.Delay(3000);
            }

            if (!_context.SimulationInputs.Any())
            {
                throw new System.Exception("SimulationInput is empty");
            }

            else if (_context.SimulationInputs.Count() == 1)
            {
                _simulationInput = _context.SimulationInputs.Find(1) ??
                                   throw new System.Exception("SimulationInput is null");
            }
            else
            {
                _simulationInput =
                    _context.SimulationInputs.Find(GetMaxPK(_context.SimulationInputs, "id")) ??
                    throw new Exception("SimulationInput is null");
            }

            if (_simulationInput.AverageConsumptionOfCars == 0)
                throw new System.Exception("AverageConsumptionOfCars is 0");
            else if (_simulationInput.ChargingStations.Count == 0)
                throw new System.Exception("ChargingStations is empty");
            else
            {
                var simulationOutput = new SimulationOutput();
                var totalEnergyCharged = 0.0;
                var theoreticalMaxPowerOutput = 0.0;
                var maxPowerOutput = 0.0;
                var concurrencyFactor = 0.0;
                var chargingValuesPerChargingStation = new List<List<double>>();
                var numberOfChargingEventsPerYear = 0;
                var numberOfChargingEventsPerMonth = 0;
                var numberOfChargingEventsPerWeek = 0;
                var numberOfChargingEventsPerDay = 0;
                var deviationOfConcurrencyFactor = 0.0;
                foreach (var chargingStation in _simulationInput.ChargingStations)
                {
                    // calculate for each charging station:
                    // 1. charging values (in kW) per chargepoint at a useful aggregation level
                    // 2. the total energy charged (in kWh)
                    // 3. the number of charging events per year/month/week/day
                    // 4. the deviation of the concurrency factor from the bonus task could be displayed


                    // use carNeedsCharging to calculate the charging values


                    var chargingValue = 0.0;
                    var chargingValues = new List<double>();

                    theoreticalMaxPowerOutput += chargingStation.ChargingPower;
                    for (var i = 0; i < _ticksPerYear; i++)
                    {
                        var numArrivals = new Random().Next(_arrivalDistribution.Length);
                        var totalPowerOutput = Math.Min(numArrivals, chargingStation.ChargingPower) *
                                               chargingStation.ChargingPower;
                        var carNeedsChargingProbability =
                            _carNeedsCharging[new Random().Next(_carNeedsCharging.Length)];
                        chargingValue = totalPowerOutput * 0.25 * carNeedsChargingProbability;
                        totalEnergyCharged += chargingValue;
                        maxPowerOutput = Math.Max(maxPowerOutput, totalPowerOutput);
                        chargingValues.Add(chargingValue);
                    }

                    // calculate the number of charging events per year/month/week/day
                    numberOfChargingEventsPerYear += (int)chargingValues.Count;
                    numberOfChargingEventsPerMonth += (int)chargingValues.Count / _ticksPerMonth;
                    numberOfChargingEventsPerWeek += (int)chargingValues.Count / _ticksPerWeek;
                    numberOfChargingEventsPerDay += (int)chargingValues.Count / _ticksPerDay;
                    // calculate the deviation of the concurrency factor
                    concurrencyFactor = maxPowerOutput / theoreticalMaxPowerOutput;
                    deviationOfConcurrencyFactor = Math.Abs(1 - concurrencyFactor);

                    chargingValuesPerChargingStation.Add(chargingValues);

                    Console.WriteLine(
                        $"Number of chargepoints: {_simulationInput.ChargingStations.IndexOf(chargingStation) + 1}");
                    Console.WriteLine($"Total energy consumed: {totalEnergyCharged} kWh");
                    Console.WriteLine($"Theoretical maximum power output: {theoreticalMaxPowerOutput} kW");
                    Console.WriteLine($"Actual maximum power output: {maxPowerOutput} kW");
                    Console.WriteLine($"Concurrency factor: {concurrencyFactor}");
                    Console.WriteLine($"Deviation of concurrency factor: {deviationOfConcurrencyFactor}");
                    Console.WriteLine();
                }

                // calculate the total concurrency factor
                concurrencyFactor = maxPowerOutput / theoreticalMaxPowerOutput;
                // calculate the deviation of the concurrency factor 
                deviationOfConcurrencyFactor = Math.Abs(1 - concurrencyFactor);

                // assign the values to the simulationOutput

                simulationOutput.TotalEnergyCharged = (int)totalEnergyCharged;
                simulationOutput.NumberOfChargingEventsPerYear = numberOfChargingEventsPerYear;
                simulationOutput.NumberOfChargingEventsPerMonth = numberOfChargingEventsPerMonth;
                simulationOutput.NumberOfChargingEventsPerWeek = numberOfChargingEventsPerWeek;
                simulationOutput.NumberOfChargingEventsPerDay = numberOfChargingEventsPerDay;
                simulationOutput.DeviationOfConcurrencyFactor = deviationOfConcurrencyFactor;
                simulationOutput.ChargingValuesPerChargingStationPerDay = chargingValuesPerChargingStation;

                return simulationOutput;
            }
        }

        public int GetMaxPK<T>(IQueryable<T> query, string pkPropertyName)
        {
            // TODO: add argument checks
            var parameter = Expression.Parameter(typeof(T));
            var body = Expression.Property(parameter, pkPropertyName);
            var lambda = Expression.Lambda<Func<T, int>>(body, parameter);
            var result = query.Max(lambda);
            return result;
        }
    }
}