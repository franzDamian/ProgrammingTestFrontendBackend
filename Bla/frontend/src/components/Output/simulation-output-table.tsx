import { Card, Typography } from "@mui/material";
import { ChargingStationBackendClient } from "../../infrastructure/generated/client.g";

export const SimulationOutputTable = (
	props: ChargingStationBackendClient.ChargerClient
) => {
	// Fetch and process data here
	// display the total energy charged
	// display the number of charging events
	// display the exemplary day charging events
	// return (
	// 	<Card>
	// 		<Typography variant="h6">Total energy charged</Typography>
	// 		{props.getOutPut().then((response) => {
	// 			response.map((item) => ({
	// 				key: item.id?.toString() ?? "",
	// 				value: item.totalEnergyCharged?.toString() ?? "",
	// 			})});
	// 		<Typography variant="h6">Number of charging events</Typography>
	// 		{props.numberOfChargingEventsPerYear}
	// 		<Typography variant="h6">Exemplary day charging events</Typography>
	// 		{
	// 			/* Display data here */
	// 			// Gererate a graph with the following axes: time (of one day), cars charging
	// 		}
	// 	</Card>
	// );
};
