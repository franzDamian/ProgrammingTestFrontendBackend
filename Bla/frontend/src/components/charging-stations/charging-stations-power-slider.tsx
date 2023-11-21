import { Slider, Typography } from "@mui/material";

type ChargingStationsPowerSliderProps = {
	readonly value: number;
	readonly setValue: (_: number) => void;
};

export const ChargingStationPowersSlider = (
	props: ChargingStationsPowerSliderProps
) => {
	return (
		<>
			<Typography variant="overline">Power of the Charging Stations</Typography>
			<Slider
				onChange={(_, v) => props.setValue(v as number)}
				value={props.value}
				valueLabelDisplay="auto"
				step={1}
				min={1}
				max={100}
			/>
		</>
	);
};
