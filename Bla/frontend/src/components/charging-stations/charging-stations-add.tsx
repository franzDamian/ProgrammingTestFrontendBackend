import { Box, Card, Stack } from "@mui/material";
import { ChargingStationAdder } from "./charging-stations-adder";
import { useState } from "react";
import ChargingStationTable from "./charging-stations-table";

//import { ChargingStationsAddList } from "./charging-stations-add-list";

export type NumberOfCharginStationWithPower = {
	count: number;
	power: number;
};

export type NumberOfCharginStationWithPowerWithId =
	NumberOfCharginStationWithPower & {
		id: number;
	};

type ChargingSTationsAddProps = {
	readonly handleSubmit: (_: NumberOfCharginStationWithPowerWithId[]) => void;
};

export const ChargingStationsAdd = (props: ChargingSTationsAddProps) => {
	const [chargingStations, setChargingStations] = useState<
		NumberOfCharginStationWithPowerWithId[]
	>([]);
	return (
		<Box>
			<Card sx={{}}>
				<Stack direction="row" sx={{}}>
					<ChargingStationAdder
						handleAdd={(value) =>
							setChargingStations((curr) => [
								...curr,
								{ count: value.count, power: value.power, id: curr.length },
							])
						}
					/>
					<ChargingStationTable
						rows={chargingStations}
						handleDelete={(value) =>
							setChargingStations((curr) => [
								...curr.filter((el) => el.id !== value.id),
							])
						}
						handleSubmit={props.handleSubmit}
					/>
				</Stack>
			</Card>
		</Box>
	);
};
