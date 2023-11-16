import { Button, List, ListItem, Stack, TableBody } from "@mui/material"

type ChargingStationsAddListProps = {
    readonly chargingStations: number[];
    readonly chargingStationsPower: number[];
}



// // TODO: add save functionality
// export const ChargingStationsAddList = (props: ChargingStationsAddListProps) => {
//     return (
//         <Stack>
//             <List>
//                 {props.map((item, index) => <ListItem key={index}>'{item}' + item</ListItem>)}
//             </List>
//             <Button variant="contained" onClick={() => saveFunction(props)}>Save List</Button>
//         </Stack>
//     )
// }

function saveFunction(list: ChargingStationsAddListProps) {
    // save the list
    
}