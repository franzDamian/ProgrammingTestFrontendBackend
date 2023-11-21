import Container from "@mui/material/Container";
import Typography from "@mui/material/Typography";
import Box from "@mui/material/Box";

import {
	ChargingEvents,
	ChargingValues,
	ExemplaryDay,
	TotalEnergyCharged,
} from "./output";

import { ChargerClient } from "./infrastructure/api";
import { useEffect, useState } from "react";
import { ChargingStationsAdd } from "./components/charging-stations/charging-stations-add";

export type ListType = { key: string; value: string };

export default function App() {
	const [chargingStations, setChargingStations] = useState<ListType[]>();
	const [chargingStationAdded, setChargingStationAdded] = useState(false);

	useEffect(() => {
		if (chargingStationAdded) {
			ChargerClient.getCharger().then((response) => {
				console.log(response);
				setChargingStations(
					response.map((item) => ({
						key: item.id?.toString() ?? "",
						value: item.chargingPower?.toString() ?? "",
					}))
				);
				setChargingStationAdded(false);
			});
		}
	}, [chargingStationAdded]);

	// const handleCharginStationAdd = useCallback(() => {
	// 	// do api stuff add chargin station, after this in .then add
	// 	setChargingStationAdded(true);
	// }, []);

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

				<ChargingStationsAdd />
				{chargingStations && <ChargingValues data={chargingStations} />}
				<ExemplaryDay />
				<TotalEnergyCharged />
				<ChargingEvents />
				{/*<Button onClick={getChargingStations}>Start Simulation</Button>(*/}
			</Box>
		</Container>
	);
}
