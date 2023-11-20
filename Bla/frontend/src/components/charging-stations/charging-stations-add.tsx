import { Box, Card, Stack } from "@mui/material";
import { ChargingStationAdder } from "./charging-stations-adder";
import { useState } from "react";
import ChargingStationTable from "./charging-stations-table";
//import { ChargingStationsAddList } from "./charging-stations-add-list";

export type NumberOfCharginStationWithPower = {
    count: number;
    power: number;
}

export type NumberOfCharginStationWithPowerWithId = NumberOfCharginStationWithPower & {
    id: number;
}


export const ChargingStationsAdd = () => {
    const [chargingStations, setChargingStations] = useState<NumberOfCharginStationWithPowerWithId[]>([]);
    return (
        <Box>
            <Card sx={{ padding: 2 }}>
                <Stack direction="row">
                    <ChargingStationAdder
                        handleAdd={(value) => setChargingStations(curr => [...curr, { count: value.count, power: value.power, id: curr.length }])} />
                    <ChargingStationTable rows={chargingStations} handleDelete={(value) => setChargingStations(curr => [...curr.filter(el => el.id !== value.id)])} />
                </Stack>
            </Card>
        </Box>);
};

