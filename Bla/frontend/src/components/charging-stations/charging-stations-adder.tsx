import { Button, Stack } from "@mui/material";
import { ChargingStationsSlider } from "./charging-stations-slider";
import { useState } from "react";

type ChargingStationAdderProps = {
    readonly handleAdd: (_: number) => void;
}


// TOOD: Add Second Slider for the amount of charging stations
export const ChargingStationAdder = (props: ChargingStationAdderProps) => {
    const [value, setValue] = useState(8);
    return (
        <Stack>
            <ChargingStationsSlider value={value} setValue={setValue} />
            <Button variant="contained" onClick={() => props.handleAdd(value)}>Add Chargin Station</Button>
        </Stack>
    );
};