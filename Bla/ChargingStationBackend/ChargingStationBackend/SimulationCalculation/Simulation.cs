using System.Text.Json.Nodes;
using Dal.Model;

namespace ChargingStationBackend.SimulationCalculation
{
    public class Simulation
    {
        private readonly SimulationInput _simulationInput;
        private readonly double[] _arrivalDistribution = {0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 2.83, 2.83, 5.66, 5.66, 5.66, 7.55, 7.55, 7.55, 10.38, 4.72, 4.72, 4.72, 0.94, 0.94 };
        private readonly double[] _carNeedsCharging = { 34.31, 4.90, 9.80, 11.76, 8.82, 11.76, 10.78, 4.90, 2.94 };
        private readonly int _ticksPerYear = 35040;
        private readonly int _ticksPerMonth = 35040/12;
        private readonly int _ticksPerWeek = 35040/54;
        private readonly int _ticksPerDay = 35040/365;

        public Simulation(SimulationInput simulationInput, Boolean random = false)
        {
            _simulationInput = simulationInput;
        }

        public SimulationOutput SimulationRun()
        {
            
            if (_simulationInput.AverageConsumptionOfCars == 0)
                throw new System.Exception("AverageConsumptionOfCars is 0");
            else if (_simulationInput.ChargingStations.Count == 0)
                throw new System.Exception("ChargingStations is empty");
            else
            {
                var simulationOutput = new SimulationOutput();
                foreach (var chargingStation in _simulationInput.ChargingStations)
                {
                    // calculate for each charging station:
                    // 1. charging values (in kW) per chargepoint at a useful aggregation level
                    // 2. the total energy charged (in kWh)
                    // 3. the number of charging events per year/month/week/day
                    // 4. the deviation of the concurrency factor from the bonus task could be displayed (if the previous bonus task was completed).
                    var chargingValue = 0.0;
                    var totalEnergyCharged = 0.0;
                    var numberOfChargingEventsPerYear = 0;
                    var numberOfChargingEventsPerMonth = 0;
                    var numberOfChargingEventsPerWeek = 0;
                    var numberOfChargingEventsPerDay = 0;
                    var deviationOfConcurrencyFactor = 0.0;
                   // TODO Fixing: var chargingValuesPerChargingStation = new ChargingValuesPerChargingStation();

                    // calculation of charging values
                    for (var i = 0; i < _ticksPerYear; i++)
                    {
                        var carNeedsCharging = _carNeedsCharging[i % _carNeedsCharging.Length];
                        var arrivalDistribution = _arrivalDistribution[i % _arrivalDistribution.Length];
                        var chargingValuePerTick = (carNeedsCharging * arrivalDistribution) / _simulationInput.AverageConsumptionOfCars;
                        chargingValue += chargingValuePerTick;
                        totalEnergyCharged += chargingValuePerTick;
                        if (chargingValuePerTick > 0)
                        {
                            numberOfChargingEventsPerYear++;
                            numberOfChargingEventsPerMonth++;
                            numberOfChargingEventsPerWeek++;
                            numberOfChargingEventsPerDay++;
                        }

                        if (i % _ticksPerMonth == 0)
                        {
                            numberOfChargingEventsPerMonth = 0;
                        }

                        if (i % _ticksPerWeek == 0)
                        {
                            numberOfChargingEventsPerWeek = 0;
                        }

                        if (i % _ticksPerDay == 0)
                        {
                            numberOfChargingEventsPerDay = 0;
                        }
                    }

                    // calculation of deviation of concurrency factor
                    var averageChargingValue = chargingValue / _ticksPerYear;
                    var sumOfSquares = 0.0;
                    for (var i = 0; i < _ticksPerYear; i++)
                    {
                        var carNeedsCharging = _carNeedsCharging[i % _carNeedsCharging.Length];
                        var arrivalDistribution = _arrivalDistribution[i % _arrivalDistribution.Length];
                        var chargingValuePerTick = (carNeedsCharging * arrivalDistribution) / _simulationInput.AverageConsumptionOfCars;
                        sumOfSquares += Math.Pow(chargingValuePerTick - averageChargingValue, 2);
                    }
                    deviationOfConcurrencyFactor = Math.Sqrt(sumOfSquares / _ticksPerYear);

                    // add values to chargingValuesPerChargingStation
                    //TODO: chargingValuesPerChargingStation.ChargingValues.Add(chargingValue);

                    // add values to simulation output
                    //TODO: simulationOutput.ChargingValuesPerChargingStation = chargingValuesPerChargingStation;
                    simulationOutput.TotalEnergyCharged = (int) totalEnergyCharged;
                    simulationOutput.NumberOfChargingEventsPerYear = numberOfChargingEventsPerYear;
                    simulationOutput.NumberOfChargingEventsPerMonth = numberOfChargingEventsPerMonth;
                    simulationOutput.NumberOfChargingEventsPerWeek = numberOfChargingEventsPerWeek;
                    simulationOutput.NumberOfChargingEventsPerDay = numberOfChargingEventsPerDay;
                    simulationOutput.DeviationOfConcurrencyFactor = deviationOfConcurrencyFactor;
                    
                }

                return simulationOutput;
            }
        }
    }
}
