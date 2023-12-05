import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";

import { ChargerClient } from "./infrastructure/api";
import { useState } from "react";
import {
	ChargingStationsAdd,
	NumberOfCharginStationWithPowerWithId,
} from "./components/charging-stations/charging-stations-add";
import { SimulationOutputChart } from "./components/Output/simulation-output-chart";
import { ChargingStationStatistic } from "./output";
import { ChargingStationBackendClient } from "./infrastructure/generated/client.g";
export type ListType = { key: string; value: string };

export default function App() {
	const [simOutput, setSimOutput] =
		useState<ChargingStationBackendClient.SimulationOutput>();

	const handleSubmit = (
		chargingStations: NumberOfCharginStationWithPowerWithId[]
	) => {
		ChargerClient.postSimulationInput({
			arrivalProbabilityMultiplier: 1,
			averageConsumptionOfCars: 18,
			chargingStations: chargingStations.flatMap((el) =>
				[...Array(el.count).keys()].map(
					(_) =>
						({
							chargingPower: el.power,
							chargingValuesForEachDayAndHour: [[0]],
						} as ChargingStationBackendClient.ChargingStationDto)
				)
			),
		} as ChargingStationBackendClient.SimulationInputDto)
			.then(() =>
				ChargerClient.getOutPut().then((response) => {
					const lastSim = response[response.length - 1];
					setSimOutput(
						new ChargingStationBackendClient.SimulationOutput({
							totalEnergyCharged: lastSim?.totalEnergyCharged,
							concurrencyFactor: lastSim?.concurrencyFactor,
							numberOfChargingEventsPerDay:
								lastSim?.numberOfChargingEventsPerDay,
							numberOfChargingEventsPerWeek:
								lastSim?.numberOfChargingEventsPerWeek,
							numberOfChargingEventsPerMonth:
								lastSim?.numberOfChargingEventsPerMonth,
							numberOfChargingEventsPerYear:
								lastSim?.numberOfChargingEventsPerYear,
							chargingStationSimulationResult:
								lastSim?.chargingStationSimulationResult,
						})
					);
				})
			)
			.catch(() => console.log("error"));
	};

	return (
		<Container>
			<Box
				sx={{
					display: "flex",
					flexDirection: "column",
					alignItems: "center",
					justifyContent: "center",
					margin: "2",
				}}
			>
				<Typography variant="h4" component="h1" gutterBottom>
					Charging Station Simulator
				</Typography>

				<ChargingStationsAdd handleSubmit={handleSubmit} />

				<SimulationOutputChart simOutput={simOutput} />
				<ChargingStationStatistic simOutput={simOutput} />
			</Box>
		</Container>
	);
}
