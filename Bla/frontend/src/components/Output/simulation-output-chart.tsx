import React, { useEffect, useState } from "react";
import { ChargerClient } from "../../infrastructure/api";
import { LineChart } from "@mui/x-charts";
import { ChargingStationBackendClient } from "../../infrastructure/generated/client.g";
import { dark } from "@mui/material/styles/createPalette";

type SimulationOutputChartProps = {
	readonly simOutput?: ChargingStationBackendClient.SimulationOutput;
};

export const SimulationOutputChart = (props: SimulationOutputChartProps) => {
	const [xAxis, setXAxis] = useState<number[]>();
	const [yAxis, setYAxis] = useState<number[]>();
	const [series, setSeries] = useState<number[][]>();

	console.log(props.simOutput);

	useEffect(() => {
		// get the last simulation output
		const newSeries = props.simOutput?.chargingStationSimulationResult?.map(
			(sr) => {
				const firstDay = sr.chargingValuesForEachDayAndHour?.[0];
				return firstDay ?? [];
			}
		);
		setSeries(newSeries);

		const firstStation = props.simOutput?.chargingStationSimulationResult?.[0];
		const firstDay = firstStation?.chargingValuesForEachDayAndHour?.[0];

		// get the values for the x-axis from new series for each charging station
		// get the values for the y-axis from new series
		setXAxis(Array.from(newSeries?.keys() ?? []) ?? []);
		setXAxis(Array.from(firstDay?.keys() ?? []));
		setYAxis(firstDay);
	}, [props.simOutput]);
	return (
		<>
			{series && xAxis && (
				<LineChart
					title="last Simulation Output"
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
