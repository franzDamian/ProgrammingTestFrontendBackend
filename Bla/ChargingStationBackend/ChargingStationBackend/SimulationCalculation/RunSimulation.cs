namespace ChargingStationBackend.SimulationCalculation
{
    public class RunSimulation
    {
        public RunSimulation(){
            double power_per_chargepoint = 11;  // kW
            int num_intervals = 35040;  // 15-minute intervals in a year

            double[] arrival_distribution = { 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 2.83, 2.83, 5.66, 5.66, 5.66, 7.55, 7.55, 7.55, 10.38, 4.72, 4.72, 4.72, 0.94, 0.94 };
            // Normalize the arrival_distribution
            double sum = arrival_distribution.Sum();
            for (int i = 0; i < arrival_distribution.Length; i++)
            {
                arrival_distribution[i] /= sum;
            }

            double[] charging_needs_distribution = { 34.31, 4.90, 9.80, 11.76, 8.82, 11.76, 10.78, 4.90, 2.94 };

            for (int num_chargepoints = 1; num_chargepoints <= 30; num_chargepoints++)  // Run the simulation for 1 to 30 chargepoints
            {
                // Simulation
                double total_energy_consumed = 0;
                double max_power_demand = 0;
                for (int i = 0; i < num_intervals; i++)
                {
                    int num_arrivals = new Random().Next(arrival_distribution.Length);
                    double total_power_demand = Math.Min(num_arrivals, num_chargepoints) * power_per_chargepoint;
                    total_energy_consumed += total_power_demand * 0.25;  // Multiply by 0.25 to convert kW to kWh for a 15-minute interval
                    max_power_demand = Math.Max(max_power_demand, total_power_demand);
                }

                // Results
                double theoretical_max_power_demand = num_chargepoints * power_per_chargepoint;
                double concurrency_factor = max_power_demand / theoretical_max_power_demand;

                Console.WriteLine($"Number of chargepoints: {num_chargepoints}");
                Console.WriteLine($"Total energy consumed: {total_energy_consumed} kWh");
                Console.WriteLine($"Theoretical maximum power demand: {theoretical_max_power_demand} kW");
                Console.WriteLine($"Actual maximum power demand: {max_power_demand} kW");
                Console.WriteLine($"Concurrency factor: {concurrency_factor}");
                Console.WriteLine();
            }

        }
    }
}
