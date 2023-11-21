import React, { useEffect, useState } from "react";
import { ChargerClient } from "../../infrastructure/api";
import { LineChart } from "@mui/x-charts";
import { ChargingStationBackendClient } from "../../infrastructure/generated/client.g";

/* Create a chart of the last 24 hours for all charging stations by getting the ChargerClient.getOutPut() and from there there the response and then the chargingStationSimulationResult. Use this to get each charging station and use the charging values per hour for one day from the charging stations then combine (x-axis: time, y-axis: cars charging) lines for each charging station and a line for the concurrency factor (which says how many charging stations are used at the same time). */

export const SimulationOutputChart = () => {
	const [simulationFinished, setSimulationFinished] = useState(false);
	useEffect(() => {
		if (simulationFinished) {
			ChargerClient.getOutPut().then((response) => {
				var data = setSimulationOutput(response);
			});
		}
	});
};

export function setSimulationFinished(finished: boolean) {
	return finished;
}
function setSimulationOutput(
	response: ChargingStationBackendClient.SimulationOutput[]
) {
	throw new Error("Function not implemented.");
}
