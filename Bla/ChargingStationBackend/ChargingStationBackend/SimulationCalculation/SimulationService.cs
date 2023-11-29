using System.ComponentModel;
using System.Diagnostics;
using Dal.Model;

namespace ChargingStationBackend.SimulationCalculation
{
    public class SimulationService
    {
        private readonly int _ticksPerHour = 4;
        private const int _ticksPerDay = 96;

        public SimulationOutput SimulationRun(SimulationInput simulationInput)
        {
            if (simulationInput.ChargingStations.Count == 0)
                throw new System.Exception("ChargingStations is empty");
            // transform the input to a list of charging stations
            var cs = simulationInput.ChargingStations.Select(cs => new SimChargingStation
            {
                Id = cs.Id,
                ChargingPower = cs.ChargingPower
            }).ToList();

            SimYear(cs, simulationInput.AverageConsumptionOfCars == 0 ? 18 : simulationInput.AverageConsumptionOfCars,
                simulationInput.ArrivalProbabilityMultiplier == 0 ? 1 : simulationInput.ArrivalProbabilityMultiplier);

            var maxPower = cs.Sum((c) => c.ChargedPower);

            var theoreticalMaxPower = cs.Sum((c) => c.ChargingPower);
            var concurrencyFactor = maxPower / theoreticalMaxPower;
            // save the simulation output to simulationOutput and return it
            var simOutput = new SimulationOutput
            {
                TotalEnergyCharged = (int)maxPower,
                NumberOfChargingEventsPerYear = cs.Sum(c => c.CountChargingEvents),
                NumberOfChargingEventsPerMonth = cs.Sum(c => c.CountChargingEventsPerDay.GetRange(0, 30).Sum()),
                NumberOfChargingEventsPerWeek = cs.Sum(c => c.CountChargingEventsPerDay.GetRange(0, 7).Sum()),
                NumberOfChargingEventsPerDay = cs.Sum(c => c.CountChargingEvents / 365),
                DeviationOfConcurrencyFactor = concurrencyFactor,
                ChargingStationSimulationResult = cs.Select(c => new ChargingStation
                {
                    Id = c.Id,
                    ChargingPower = c.ChargingPower,
                    ChargingValuesForEachDayAndHour = new List<List<double>>(GetListOfChargedPowerPerHour(c))
                }).ToList()
            };
            return simOutput;
        }

        private List<List<double>> GetListOfChargedPowerPerHour(SimChargingStation simChargingStation)
        {
            var chargedPowerPerHourPerDay = new List<List<double>>();
            for (var i = 0; i < 365; ++i)
            {
                var chargedPowerPerHour = new List<double>();
                for (var j = 0; j < 24; ++j)
                {
                    // get the charged power per hour by summing up the charged power per tick in that hour
                    chargedPowerPerHour.Add(simChargingStation.ChargedPowerPerTick
                        .GetRange(i * _ticksPerDay + j * _ticksPerHour, _ticksPerHour).Sum());
                }

                chargedPowerPerHourPerDay.Add(chargedPowerPerHour);
            }

            return chargedPowerPerHourPerDay;
        }


        internal void SimYear(List<SimChargingStation> list,
            int? averageCarConsumptionPer100Km, double arrivalMultiplier)
        {
            Enumerable.Range(0, 365).ToList().ForEach(_ =>
                SimulateDay(list, averageCarConsumptionPer100Km, arrivalMultiplier));
        }


        internal void SimulateDay(List<SimChargingStation> list,
            int? averageCarConsumptionPer100Km, double arrivalMultiplier)
        {
            // the arrival distribution per hour
            var arrivalDistribution = new List<double>
            {
                .0094, .0094, .0094, .0094, .0094, .0094, .0094, .0094, .0283, .0283, .0566, .0566, .0566, .0755, .0755,
                .0755, .1038, .1038, .1038, .0472,
                .0472, .0472, .0094, .0094
            };
            // for each tick
            for (var i = 0; i < _ticksPerDay; i++)
            {
                // simulate the tick and get the charged power per charging station
                SimTick(list, arrivalDistribution[i / _ticksPerHour] * (double)arrivalMultiplier,
                    averageCarConsumptionPer100Km);
            }

            // save the charging stations values per day
            foreach (var cs in list)
            {
                cs.SetChargedPowerPerDay();
                cs.SetChargingEventsPerDay();
            }
        }

        internal static List<double> SimTick(List<SimChargingStation> list, double arrivalProbability,
            int? averageCarConsumptionPer100Km)
        {
            var chargedPerStation = new List<double>();
            // for each charging station
            foreach (var cs in list)
            {
                // check if a car arrives
                cs.Arrive(arrivalProbability, averageCarConsumptionPer100Km);
                // charge the car currently charging
                chargedPerStation.Add(cs.ChargeCar());
            }

            return chargedPerStation; // return the charged power per tick per charging station
        }
    }

    public class SimChargingStation
    {
        public int Id { get; set; }
        public int ChargingPower { get; set; } // in kw per tick
        private Car? Car { get; set; }
        public int CountChargingEvents { get; private set; } = 0;
        public double ChargedPower { get; private set; } = 0; // in kw
        public List<int> CountChargingEventsPerDay { get; private set; } = new List<int>();
        public List<double> ChargedPowerPerDay { get; private set; } = new List<double>();
        public List<double> ChargedPowerPerTick { get; private set; } = new List<double>();


        public void SetChargingEventsPerDay()
        {
            var countChargingEventsInPast = CountChargingEventsPerDay.Sum();
            var currentCountChargingEvents = CountChargingEvents - countChargingEventsInPast;
            CountChargingEventsPerDay.Add(currentCountChargingEvents);
        }

        public void SetChargedPowerPerDay()
        {
            var chargedPowerInPast = ChargedPowerPerDay.Sum();
            var currentChargedPower = ChargedPower - chargedPowerInPast;
            ChargedPowerPerDay.Add(currentChargedPower);
        }

        public double ChargeCar()
        {
            if (Car is null)
            {
                ChargedPowerPerTick.Add(0);
                return 0;
            }

            var chargedPower = Math.Min(ChargingPower / 4.0, Car.ChargingDemand);
            Car.ChargingDemand -= ChargingPower; // charging power is in kw and per tick
            if (Car.ChargingDemand <= 0)
            {
                ChargedPowerPerTick.Add(0);
                Car = null;
            }

            ChargedPowerPerTick.Add(chargedPower);
            ChargedPower += chargedPower;
            return chargedPower;
        }

        public void Arrive(double arrivalProbability, int? averageCarConsumptionPer100Km)
        {
            if (Car is not null)
            {
                return;
            }

            if (new Random().NextDouble() > arrivalProbability)
            {
                return;
            }

            var car = averageCarConsumptionPer100Km is null ? new Car() : new Car((int)averageCarConsumptionPer100Km);
            if (car.ChargingDemand == 0)
            {
                return;
            }

            Car = car;
            CountChargingEvents++;
        }
    }

    public class Car
    {
        public Car(int consumptionPer100Km = 18)
        {
            ChargingDemand = GetChargingDemandInKw(consumptionPer100Km);
        }

        public double ChargingDemand { get; set; }

        private static double GetChargingDemandInKw(int consumptionPer100Km)
        {
            var random = new Random().NextDouble();
            var chargingDemandInKm = random switch
            {
                < 0.3431 => 0.0,
                < 0.3921 => 5.0,
                < 0.4901 => 10.0,
                < 0.6077 => 20.0,
                < 0.6959 => 30.0,
                < 0.8135 => 50.0,
                < 0.9213 => 100.0,
                < 0.9703 => 200.0,
                < 0.9997 => 300.0,
                _ => 0 // statistic outlier 1-0.9997
            };
            return chargingDemandInKm / 100.0 * consumptionPer100Km;
        }
    }
}