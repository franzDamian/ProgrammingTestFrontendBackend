# Charging Station Backend

This project simulates the energy consumption and power demand of an electric vehicle (EV) charging station. The simulation takes into account the probability distributions of EV arrivals and their charging needs, as well as the number of available chargepoints.

## Usage

You can use the provided C# code to run the simulation for 1 to 30 chargepoints. The simulation outputs the following results for each `num_chargepoints` value:

- `Number of chargepoints`: The number of available chargepoints.
- `Total energy consumed`: The total energy consumed by all EVs in kWh.
- `Theoretical maximum power demand`: The maximum power demand if all chargepoints were used simultaneously.
- `Actual maximum power demand`: The maximum power demand observed during the simulation.
- `Concurrency factor`: The ratio of actual maximum power demand to theoretical maximum power demand.

## Code

The simulation code is written in C#. It uses the following variables:

- `power_per_chargepoint`: The power rating of each chargepoint in kW.
- `num_intervals`: The number of 15-minute intervals in a year.
- `arrival_distribution`: The probability distribution of EV arrivals per 15-minute interval.
- `charging_needs_distribution`: The probability distribution of EV charging needs in kWh.

The simulation code is located in the `Program.cs` file.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
