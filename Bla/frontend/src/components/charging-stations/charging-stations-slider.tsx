import { Box, Card, Slider, Typography } from "@mui/material";

type ChargingStationsSliderProps = {
    readonly value: number;
    readonly setValue: (_: number) => void;
}

export const ChargingStationsSlider = (props: ChargingStationsSliderProps) => {
    return (
        <>
                <Typography variant="overline">Charging Station</Typography>
                    <Slider onChange={(_, v) => props.setValue(v as number)}
                value={props.value}
                        valueLabelDisplay="auto"
                        step={1}
                        min={1}
                        max={30}
                        />
          </>  
    );
  }