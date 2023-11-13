using System.ComponentModel;
using System.Diagnostics;
using Dal.Model;

namespace ChargingStationBackend.SimulationCalculation
{
    public class TempSim
    {
        private int _ticksPerHour = 4;
        private const int _ticksPerDay = 96;

        public TempSim()
        {
        }

        public void SimYear(List<SimChargingStation> list,
            int? averageCarConsumptionPer100Km)
        {
            Enumerable.Range(0, 365).ToList().ForEach(_ =>
                SimulateDay(list, averageCarConsumptionPer100Km));
        }

        public void SimulateDay(List<SimChargingStation> list,
            int? averageCarConsumptionPer100Km)
        {
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
                SimTick(list, arrivalDistribution[i / _ticksPerHour], averageCarConsumptionPer100Km);
            }

            // save the charging stations values per day
            foreach (var cs in list)
            {
                cs.SetChargedPowerPerDay();
                cs.SetChargingEventsPerDay();
            }
        }

        public List<double> SimTick(List<SimChargingStation> list, double arrivalProbability,
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


                // save the charged power
                // save the number of charging events
                // save the theoretical maximum used charging power
                // save the deviation of the concurrency factor
                // save the total energy charged
                // save the charging values per charging station per day
                // save the actual maximum used charging power
            }

            return chargedPerStation; // return the charged power per tick per charging station
        }

        public SimulationOutput SimulationRun()
        {
            var cs = Enumerable.Range(0, 1).Select(_ => new SimChargingStation { ChargingPower = 11 }).ToList();
            new SimChargingStation { ChargingPower = 11 };

            SimYear(cs, null);

            var maxPower = 0.0;
            for (var i = 0; i < _ticksPerDay * 365; ++i)
            {
                maxPower = Math.Max(cs.Sum(c => c.ChargedPowerPerTick[i]), maxPower);
            }

            var theoreticalMaxPower = cs.Sum((c) => c.ChargingPower);
            var concurrencyFactor = maxPower / theoreticalMaxPower;
            return new SimulationOutput();
        }
    }

    public class SimChargingStation
    {
        public int ChargingPower { get; set; } // in kw per tick
        private Car? Car { get; set; }
        public int CountChargingEvents { get; private set; } = 0;
        public double ChargedPower { get; private set; } = 0;
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

            var chargedPower = Math.Min(ChargingPower, Car.ChargingDemand);
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

        private double GetChargingDemandInKw(int consumptionPer100Km)
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