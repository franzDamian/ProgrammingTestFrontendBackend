import { Box, Card, Stack } from "@mui/material";
import { ChargingStationAdder } from "./charging-stations-adder";
import { useState } from "react";
import ChargingStationTable from "./charging-stations-table";
//import { ChargingStationsAddList } from "./charging-stations-add-list";

export type NumberOfCharginStationWithPower = {
    count: number;
    power: number;
}

export const ChargingStationsAdd = () => {
    const [chargingStations, setChargingStations] = useState<NumberOfCharginStationWithPower[]>([]);
    return (
        <Box>
            <Card sx={{ padding: 2 }}>
                <Stack direction="row">
                    <ChargingStationAdder
                        handleAdd={(value) => setChargingStations(curr => [...curr, value])} />
                    <ChargingStationTable rows={chargingStations} />
                </Stack>
            </Card>
        </Box>);
};

