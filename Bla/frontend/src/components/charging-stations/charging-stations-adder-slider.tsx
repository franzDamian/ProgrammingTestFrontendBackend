import { Slider, Typography } from "@mui/material";

type ChargingStationsNumberSliderProps = {
	readonly value: number;
	readonly setValue: (_: number) => void;
};

export const ChargingStationsNumberSlider = (
	props: ChargingStationsNumberSliderProps
) => {
	return (
		<>
			<Typography variant="overline">Number of Charging Stations</Typography>
			<Slider
				onChange={(_, v) => props.setValue(v as number)}
				value={props.value}
				valueLabelDisplay="auto"
				step={1}
				min={1}
				max={50}
			/>
		</>
	);
};
