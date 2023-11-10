import { Button, List, ListItem, Stack } from "@mui/material"

type ChargingStationsAddListProps = {
    readonly chargingStations: number[];
}


// TODO: add save functionality
export const ChargingStationsAddList = (props: ChargingStationsAddListProps) => {
    return (
        <Stack>
        <List>
            {props.chargingStations.map((item, index) => <ListItem key={index}>{item}</ListItem>)}
            </List>
            <Button variant="contained" onClick={() => alert("TODO")}>Save List</Button>
            </Stack>
    )
}