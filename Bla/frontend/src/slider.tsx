import { Box, Button, Card, Slider, Typography } from "@mui/material";
import { useState } from "react";

type DiscreteSliderProps = Readonly<{
    max: number;
    min: number;
    initial: number;
    steps?: number;
    title: string;
    unit?: string;
}>;

export default function DiscreteSlider(props: DiscreteSliderProps) {
    const [value, setValue] = useState(props.initial);
    return (
        <Box sx={{ width: 300 }}>
            <Card sx={{padding:2}}>
                <Typography variant="overline">{props.title} {value} {props.unit??""}</Typography>
                    <Slider onChange={(_, v) => setValue(v as number)}
                        aria-label="Charging stations"
                        defaultValue={props.initial}
                        valueLabelDisplay="auto"
                        step={props.steps??1}
                        min={props.min}
                        max={props.max}
                        />
            </Card>
      </Box>
    );
  }