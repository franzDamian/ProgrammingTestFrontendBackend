import React, { useEffect, useState } from "react";
import { ChargerClient } from "../../infrastructure/api";
import { LineChart } from "@mui/x-charts";
import { ChargingStationBackendClient } from "../../infrastructure/generated/client.g";
import { dark } from "@mui/material/styles/createPalette";

/* Create a chart of the last 24 hours for all charging stations by getting the ChargerClient.getOutPut() and from there there the response and then the chargingStationSimulationResult. Use this to get each charging station and use the charging values per hour for one day from the charging stations then combine (x-axis: time, y-axis: cars charging) lines for each charging station and a line for the concurrency factor (which says how many charging stations are used at the same time). */

export const SimulationOutputChart = () => {
	const [xAxis, setXAxis] = useState<number[]>();
	const [yAxis, setYAxis] = useState<number[]>();
	const [series, setSeries] = useState<number[][]>();
	useEffect(() => {
		ChargerClient.getOutPut().then((response) => {
			const firstSim = response[0];
			console.log(firstSim.chargingStationSimulationResult);
			const newSeries = firstSim.chargingStationSimulationResult?.map((sr) => {
				const firstDay = sr.chargingValuesForEachDayAndHour?.[0];
				return firstDay ?? [];
			});
			console.log(newSeries);
			setSeries([newSeries?.[0] ?? []]);

			const firstStation = firstSim.chargingStationSimulationResult?.[0];
			const firstDay = firstStation?.chargingValuesForEachDayAndHour?.[0];
			setXAxis(Array.from(firstDay?.keys() ?? []));
			setYAxis(firstDay);
			console.log(firstDay);
		});
	}, []);
	return (
		<>
			{series && xAxis && (
				<LineChart
					series={
						series?.map((v, i) => ({ data: v, label: i.toString() })) ?? []
					}
					xAxis={[{ scaleType: "point", data: xAxis }]}
					width={500}
					height={300}
				/>
			)}
		</>
	);
};

export function setSimulationFinished(finished: boolean) {
	return finished;
}
function setSimulationOutput(
	response: ChargingStationBackendClient.SimulationOutput[]
) {
	if (response === undefined) {
		throw new Error("No simulation output found");
	}
	if (response.length === 0) {
		throw new Error("No simulation output found");
	}
	if (response.length === 1) {
		return response[0];
	}
	// return last simulation output
	return response.find((item) => {
		return item === response[response.length - 1];
	});
}
