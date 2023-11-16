import { Button, Stack } from "@mui/material";
import { ChargingStationsNumberSlider } from "./charging-stations-adder-slider";
import { ChargingStationPowersSlider } from "./charging-stations-power-slider";
import { useState } from "react";
import { NumberOfCharginStationWithPower } from "./charging-stations-add";

type ChargingStationAdderProps = {
    readonly handleAdd: (_: NumberOfCharginStationWithPower) => void;
}

// TOOD: Add Second Slider for the amount of charging stations
export const ChargingStationAdder = (props: ChargingStationAdderProps) => {
    const [count, setCount] = useState(8);
    const [power, setPower] = useState(11);
    return (
        <Stack>
            <ChargingStationsNumberSlider value={count} setValue={setCount} />
            <ChargingStationPowersSlider value={power} setValue={setPower} />
            <Button variant="contained" onClick={() => props.handleAdd({count: count, power: power})}>Add Charging Stations</Button>
        </Stack>
    );
};
