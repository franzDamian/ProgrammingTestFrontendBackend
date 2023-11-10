import { Box, Card, Stack } from "@mui/material";
import { ChargingStationAdder } from "./charging-stations-adder";
import { useState } from "react";
import { ChargingStationsAddList } from "./charging-stations-add-list";

export const ChargingStationsAdd = () => {

    const [chargingStations, setChargingStations] = useState<number[]>([]);

    return (
        <Box>
            <Card sx={{padding:2}}>
        <Stack direction="row">
            <ChargingStationAdder handleAdd={(value) => setChargingStations(currState => [...currState, value])} />
            <ChargingStationsAddList chargingStations={chargingStations} />
            </Stack>
            </Card>
      </Box>);
};