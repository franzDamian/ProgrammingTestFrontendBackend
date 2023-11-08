using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Model
{
    public class SimulationInput
    {
        /*  the number of charge points
            a multiplier for the arrival probability to increase the amount of cars arriving to charge(20-200%, default: 100%)
            the consumption of the cars (default: 18 kWh)
            the charging power per chargepoint (default: 11 kW)
        */
        public int id { get; set; }
        public List<ChargingStation> ChargingStations { get; set; } = new List<ChargingStation>();
        public int AverageConsumptionOfCars { get; set; }
        public int ArrivalProbabilityMultiplier { get; set; }
    }
}
