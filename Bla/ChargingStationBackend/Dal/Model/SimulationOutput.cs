using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Model
{
    public class SimulationOutput
    {
        /*
            For the output, you could visualize:
                The charging values (in kW) per chargepoint at a useful aggregation level
                An exemplary day
                The total energy charged (in kWh)
                The number of charging events per year/month/week/day
                The deviation of the concurrency factor from the bonus task could be displayed (ifthe previous bonus task was completed).
         */

        public int Id { get; set; }
        public List<Tuple<ChargingStation, double>> ChargingValuesPerChargingStation { get; set; } = new List<Tuple<ChargingStation, double>>(); 
        public int TotalEnergyCharged { get; set; }
        public int NumberOfChargingEventsPerYear { get; set; }
        public int NumberOfChargingEventsPerMonth { get; set; }
        public int NumberOfChargingEventsPerWeek { get; set; }
        public int NumberOfChargingEventsPerDay { get; set; }
        public double DeviationOfConcurrencyFactor { get; set; }
    }
}
