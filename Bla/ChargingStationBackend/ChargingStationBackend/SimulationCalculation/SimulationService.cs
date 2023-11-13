using System;
using System.Linq.Expressions;
using System.Text.Json.Nodes;
using Dal;
using Dal.Model;
using Microsoft.EntityFrameworkCore;

namespace ChargingStationBackend.SimulationCalculation
{
    public class SimulationService
    {
        // _arrivalDistribution is the probability that a car arrives per hour (24 hours)
        private readonly double[] _arrivalDistribution =
        {
            .0094, .0094, .0094, .0094, .0094, .0094, .0094, .0094, .0283, .0283, .0566, .0566, .0566, .0755, .0755,
            .0755, .1038, .0472,
            .0472, .0472, .0094, .0094
        };

        // _carNeedsCharging is the probability that a car needs charging and how much energy it needs in Kilometers (0 km, 5km, 10 km, 20 km, 30 km, 50 km, 100 km, 200 km, 300 km)
        private readonly double[] _carChargingDemandInPercent =
            { .3431, .0490, .0980, .1176, .0882, .1176, .1078, .0490, .0294 };

        private readonly double[] _carChargingDemandInKm =
            { 0, 5, 10, 20, 30, 50, 100, 200, 300 };

        private readonly int _ticksPerYear = 35040;
        private readonly int _ticksPerMonth = 35040 / 12;
        private readonly int _ticksPerWeek = 35040 / 54;
        private readonly int _ticksPerDay = 35040 / 365;
        private int _daysPerYear = 365;


        public SimulationOutput SimulationRun(SimulationInput simulationInput)
        {
            if (simulationInput.AverageConsumptionOfCars == 0)
                throw new System.Exception("AverageConsumptionOfCars is 0");
            if (simulationInput.ChargingStations.Count == 0)
                throw new System.Exception("ChargingStations is empty");


            var averageConsumptionOfCarsPer100Km = simulationInput.AverageConsumptionOfCars;
            var simulationOutput = new SimulationOutput();
            var totalEnergyCharged = 0.0;
            var theoreticalMaxPowerOutput = 0.0;
            var actualMaxDemand = 0.0;
            var concurrencyFactor = 0.0;
            var chargingValuesPerChargingStation = new List<List<double>>();
            var numberOfChargingEventsPerYear = 0;
            var numberOfChargingEventsPerMonth = 0;
            var numberOfChargingEventsPerWeek = 0;
            var numberOfChargingEventsPerDay = 0;
            var deviationOfConcurrencyFactor = 0.0;
            var chargedByCurrentCar = 0.0;
            var chargingNeedInKiloWatt = 0.0;


            foreach (var chargingStation in simulationInput.ChargingStations)
            {
                var chargingValuePerDay = 0.0;


                var chargingValues = new List<double>();
                var chargingStationIsUsed = false;
                theoreticalMaxPowerOutput += chargingStation.ChargingPower;

                // calculate the charging values per day for one charging station for one year
                for (var i = 0; i < _daysPerYear; i++)
                {
                    // calculate the charging value per tick
                    for (var j = 0; j < _ticksPerDay / 4; j++)
                    {
                        // calculate the probability that a car needs charging and how long it needs to charging
                        // depending on the power consumption of the car and the charging power of the charging station
                        // and the _carChargingDemandInPercent array
                        var demandIndex = new Random().Next(_carChargingDemandInPercent.Length);

                        var carNeedsChargingProbability =
                            _carChargingDemandInPercent[demandIndex];
                        chargingNeedInKiloWatt = (_carChargingDemandInKm[demandIndex] / 100) * 18;
                        // calculate the arrival probability per tick and multiply it with the arrival probability multiplier,
                        // by getting the probability from the arrival distribution for each hour of the day
                        var arrivalProbabilityPerHour =
                            _arrivalDistribution[j] * simulationInput.ArrivalProbabilityMultiplier;

                        // decide randomly if a car arrives depending on the arrival probability per hour and the arrival probability multiplier
                        var carArrives = new Random().NextDouble() < arrivalProbabilityPerHour;
                        if ((carArrives && !chargingStationIsUsed) || chargingStationIsUsed)
                        {
                            // decide if the car needs charging with random and the carNeedsChargingProbability
                            if (new Random().NextDouble() < carNeedsChargingProbability || chargingStationIsUsed)
                            {
                                // for each tick, calculate the charging value per tick and if the charging station is used 
                                while (chargingNeedInKiloWatt > 0) // hääöääääääääää
                                {
                                    for (var k = 0; k < 4; k++)
                                    {
                                        var currentPower = chargingStation.ChargingPower / 4.0;
                                        // calculate the charged energy with the minimum of current power and the charging chargingNeedInKiloWatt
                                        chargedByCurrentCar += Math.Min(currentPower, chargingNeedInKiloWatt);
                                        // calculate the charging value per day with the minimum of current power and the charging chargingNeedInKiloWatt
                                        chargingValuePerDay += Math.Min(currentPower, chargingNeedInKiloWatt);
                                        chargingNeedInKiloWatt -= currentPower;

                                        totalEnergyCharged += Math.Min(currentPower, chargingNeedInKiloWatt);
                                        numberOfChargingEventsPerYear++; // ????????
                                        if (!(chargingNeedInKiloWatt > 0))
                                        {
                                            // if the car doesn't need charging anymore, look for the next car that needs charging
                                            // and calculate the charging value per tick
                                            carArrives = new Random().NextDouble() < arrivalProbabilityPerHour;
                                            if (carArrives)
                                            {
                                                chargingStationIsUsed = true;
                                                demandIndex = new Random().Next(_carChargingDemandInPercent.Length);
                                                carNeedsChargingProbability =
                                                    _carChargingDemandInPercent[demandIndex];
                                                chargingNeedInKiloWatt =
                                                    (_carChargingDemandInKm[demandIndex] / 100) * 18;
                                            }
                                            else
                                            {
                                                // if the car is charged, calculate if a new car arrives
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    chargingValues.Add(chargingValuePerDay);
                    // calculate the actual max demand of all charged cars per day so far
                    actualMaxDemand = chargingValuePerDay > actualMaxDemand ? chargingValuePerDay : actualMaxDemand;

                    chargingValuePerDay = 0;
                }

                // save all charging values per charging station per day in a list
                chargingValuesPerChargingStation.Add(chargingValues);
                // calculate the actual maximum power output of all charging stations looked at so far


                // calculate the number of charging events per year/month/week/day
                numberOfChargingEventsPerYear =
                    numberOfChargingEventsPerMonth += (int)chargingValues.Count / _ticksPerMonth;
                numberOfChargingEventsPerWeek += (int)chargingValues.Count / _ticksPerWeek;
                numberOfChargingEventsPerDay += (int)chargingValues.Count / _ticksPerDay;
                // calculate the deviation of the concurrency factor
                concurrencyFactor = actualMaxDemand / theoreticalMaxPowerOutput;
                deviationOfConcurrencyFactor = Math.Abs(1 - concurrencyFactor);

                chargingValuesPerChargingStation.Add(chargingValues);

                Console.WriteLine(
                    $"Number of chargepoints: {simulationInput.ChargingStations.IndexOf(chargingStation) + 1}");
                Console.WriteLine($"Total energy consumed: {totalEnergyCharged} kWh");
                Console.WriteLine($"Theoretical maximum power output: {theoreticalMaxPowerOutput} kW");
                Console.WriteLine($"Actual maximum power output: {actualMaxDemand} kW");
                Console.WriteLine($"Concurrency factor: {concurrencyFactor}");
                Console.WriteLine($"Deviation of concurrency factor: {deviationOfConcurrencyFactor}");
                Console.WriteLine();
            }

            // calculate the total concurrency factor
            concurrencyFactor = actualMaxDemand / theoreticalMaxPowerOutput;
            // calculate the deviation of the concurrency factor 
            deviationOfConcurrencyFactor = Math.Abs(1 - concurrencyFactor);

            // assign the values to the simulationOutput

            simulationOutput.TotalEnergyCharged = (int)totalEnergyCharged;
            simulationOutput.NumberOfChargingEventsPerYear = numberOfChargingEventsPerYear;
            simulationOutput.NumberOfChargingEventsPerMonth = numberOfChargingEventsPerMonth;
            simulationOutput.NumberOfChargingEventsPerWeek = numberOfChargingEventsPerWeek;
            simulationOutput.NumberOfChargingEventsPerDay = numberOfChargingEventsPerDay;
            simulationOutput.DeviationOfConcurrencyFactor = deviationOfConcurrencyFactor;
            simulationOutput.ChargingValuesPerChargingStationPerDay = new List<List<double>>();

            return simulationOutput;
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