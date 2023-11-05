import numpy as np

# Constants
num_chargepoints = 20
power_per_chargepoint = 11  # kW
num_intervals = 35040  # 15-minute intervals in a year

# Probability distributions
arrival_distribution = [0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 0.94, 2.83, 2.83, 5.66, 5.66, 5.66, 7.55, 7.55, 7.55, 10.38, 4.72, 4.72, 4.72, 0.94, 0.94]
#probability distribution of an arriving EV's charging needs
charging_needs_distribution = [34.31, 4.90, 9.80, 11.76, 8.82, 11.76, 10.78, 4.90, 2.94]


# Simulation
total_energy_consumed = 0
max_power_demand = 0
for _ in range(num_intervals):
    num_arrivals = np.random.choice(len(arrival_distribution), p=arrival_distribution)
    total_power_demand = min(num_arrivals, num_chargepoints) * power_per_chargepoint
    total_energy_consumed += total_power_demand * 0.25  # Multiply by 0.25 to convert kW to kWh for a 15-minute interval
    max_power_demand = max(max_power_demand, total_power_demand)

# Results
theoretical_max_power_demand = num_chargepoints * power_per_chargepoint
concurrency_factor = max_power_demand / theoretical_max_power_demand

print(f"Total energy consumed: {total_energy_consumed} kWh")
print(f"Theoretical maximum power demand: {theoretical_max_power_demand} kW")
print(f"Actual maximum power demand: {max_power_demand} kW")
print(f"Concurrency factor: {concurrency_factor}")