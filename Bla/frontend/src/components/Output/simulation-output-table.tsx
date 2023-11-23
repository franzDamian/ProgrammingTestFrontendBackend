// import { Card, Stack, Typography } from "@mui/material";
// import { ChargingStationBackendClient } from "../../infrastructure/generated/client.g";

// export const SimulationOutputTable = (
// 	props: ChargingStationBackendClient.ChargerClient
// ) => {
// 	// Fetch and process data here
// 	// display the total energy charged
// 	// display the number of charging events
// 	// display the exemplary day charging events

// 	return (
// 		<Card>
// 			<Stack>
// 				<Typography>Simulation Output</Typography>
// 				<Typography>Total Energy Charged: </Typography>
// 				<Typography>Number of Charging Events: </Typography>
// 				{props.getOutPut().then((response) => {
// 					response.map((item) => (
// 						<Typography>
// 							Charging Station {item.id}: {item.chargingPower}
// 						</Typography>
// 					));
// 				})}
// 				<Typography>Exemplary Day Charging Events: </Typography>
// 			</Stack>
// 		</Card>
// 	);
// };
