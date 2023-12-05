import { Button, Stack } from "@mui/material";
import { ChargingStationsNumberSlider } from "./charging-stations-adder-slider";
import { ChargingStationPowersSlider } from "./charging-stations-power-slider";
import { useState } from "react";
import { NumberOfCharginStationWithPower } from "./charging-stations-add";
import { FormPropsTextFields } from "../other-components/inputBox";

type ChargingStationAdderProps = {
	readonly handleAdd: (_: NumberOfCharginStationWithPower) => void;
};

// TOOD: Add Second Slider for the amount of charging stations
export const ChargingStationAdder = (props: ChargingStationAdderProps) => {
	const [count, setCount] = useState(8);
	const [power, setPower] = useState(11);
	return (
		<Stack
			spacing={2}
			sx={{
				padding: 2,
			}}
		>
			<FormPropsTextFields
				label="Avg. consumption cf the per 100km"
				type="number"
				defaultValue={18}
				min={1}
				max={50}
				step={1}
				endAdornment="kWh"
				shrink={true}
			/>
			<FormPropsTextFields
				label="Arrival probility multiplier of cars in %"
				type="number"
				defaultValue={100}
				min={1}
				max={200}
				step={1}
				endAdornment="%"
				shrink={true}
			/>
			<ChargingStationsNumberSlider value={count} setValue={setCount} />
			<ChargingStationPowersSlider value={power} setValue={setPower} />
			<Button
				variant="contained"
				onClick={() => props.handleAdd({ count: count, power: power })}
			>
				Add Charging Stations
			</Button>
		</Stack>
	);
};
