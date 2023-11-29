import { useEffect, useState } from "react";
import { ChargingStationBackendClient } from "../src/infrastructure/generated/client.g";
import { ChargerClient } from "./infrastructure/api";
import {
	Box,
	Table,
	TableBody,
	TableCell,
	TableContainer,
	TableHead,
	TableRow,
} from "@mui/material";

type ChargingStationStatisticProps = {
	readonly simOutput?: ChargingStationBackendClient.SimulationOutput;
};

export const ChargingStationStatistic = (
	props: ChargingStationStatisticProps
) => {
	return (
		<>
			<Box
				sx={{
					display: "flex",
					flexDirection: "column",
					alignItems: "center",
					justifyContent: "center",
					margin: "2",
				}}
			>
				<TableContainer>
					<Table sx={{ maxWidth: 800 }}>
						<TableHead>
							<TableRow>
								<TableCell>Statistic</TableCell>
								<TableCell>Value</TableCell>
							</TableRow>
						</TableHead>
						<TableBody>
							<TableRow>
								<TableCell>Total Energy Charged </TableCell>
								<TableCell>{props?.simOutput?.totalEnergyCharged} KW</TableCell>
							</TableRow>
							<TableRow>
								<TableCell>Year</TableCell>
								<TableCell>
									{props?.simOutput?.numberOfChargingEventsPerYear}
								</TableCell>
							</TableRow>
							<TableRow>
								<TableCell>Month</TableCell>
								<TableCell>
									{props?.simOutput?.numberOfChargingEventsPerMonth}
								</TableCell>
							</TableRow>
							<TableRow>
								<TableCell>Week</TableCell>
								<TableCell>
									{props?.simOutput?.numberOfChargingEventsPerWeek}
								</TableCell>
							</TableRow>
							<TableRow>
								<TableCell>Day</TableCell>
								<TableCell>
									{props?.simOutput?.numberOfChargingEventsPerDay}
								</TableCell>
							</TableRow>
							<TableRow>
								<TableCell>Deviation of Concurrency Factor</TableCell>
								<TableCell>
									{props?.simOutput?.deviationOfConcurrencyFactor}
								</TableCell>
							</TableRow>
						</TableBody>
					</Table>
				</TableContainer>
			</Box>
		</>
	);
};
