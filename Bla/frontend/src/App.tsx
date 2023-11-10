import * as React from 'react';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Link from '@mui/material/Link';
import DiscreteSlider from './slider';
import { List } from './list';
import FormPropsTextFields from './inputBox';
import { ChargingEvents, ChargingValues, ExemplaryDay, TotalEnergyCharged } from './output';
import { Button } from '@mui/material';
import { ChargerClient } from './infrastructure/api';
import { useCallback, useEffect, useState } from 'react';
import { ChargingStationsAdd } from './components/charging-stations/charging-stations-add';

export type ListType = { key: string, value: string };

const testList: ListType[] = [
    { key: "chargepoint", value: "11 kw" },
    { key: "bla2", value: "blabla2" }
]

function Copyright() {
  return (
    <Typography variant="body2" color="text.secondary" align="center">
      {'Copyright © '}
      <Link color="inherit" href="https://mui.com/">
        Your Website
      </Link>{' '}
      {new Date().getFullYear()}.
    </Typography>
  );
}

export default function App() {
    const [chargingStations, setChargingStations] = useState<ListType[]>();
    const [chargingStationAdded, setChargingStationAdded] = useState(false);

    useEffect(() => {
        if (chargingStationAdded) {
            ChargerClient.getCharger().then((response) => {
                console.log(response);
                setChargingStations(response.map((item) => ({ key: item.id?.toString() ?? "", value: item.chargingPower?.toString() ?? "" })));
                setChargingStationAdded(false);
            });
        }
    }, [chargingStationAdded]);

    const handleCharginStationAdd = useCallback(() => {
        // do api stuff add chargin station, after this in .then add
        setChargingStationAdded(true);
    }, []);

    return (
    <Container maxWidth="sm">
      <Box sx={{ my: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Material UI Vite.js example in TypeScript
        </Typography>
        <DiscreteSlider initial={8} max={30} min={1} title='Charging Stations' />
        <DiscreteSlider initial={100} max={200} min={20} steps={10} title='Multiplier for arival probalilty' unit='%'/>
                <FormPropsTextFields />
                <Button variant="contained" onClick={() => handleCharginStationAdd()}>Add CharginStations</Button>
        {chargingStations && <ChargingValues data={chargingStations} />}
        <ExemplaryDay />
        <TotalEnergyCharged />
        <ChargingEvents />
        {/*<Button onClick={getChargingStations}>Start Simulation</Button>(*/}
                <Copyright />
                
                <ChargingStationsAdd />
      </Box>
    </Container>
  );
}

interface SimulationOutput {
  id: number;
  chargingValuesPerChargingStationPerDay: number[][];
  totalEnergyCharged: number;
  numberOfChargingEventsPerYear: number;
  numberOfChargingEventsPerMonth: number;
  numberOfChargingEventsPerWeek: number;
  numberOfChargingEventsPerDay: number;
  deviationOfConcurrencyFactor: number;
}

type ChargingStation = {
    id: number;
    chargingPower: number;
}

interface SimulationInput {
    chargingStations: ChargingStation[];
    numberOfChargingStations: number;
    multiplierForArrivalProbability: number;
    consumptionOfCars: number;
    numberOfDays: number;
    numberOfIterations: number;
}

// handle the output of the simulation
function handleSimulationOutput() {
  // fetch the data from the backend with swagger and display it curl -X 'GET' \'https://localhost:7067/Charger/getOutput' \-H 'accept: application/json'
  var output: SimulationOutput;
  fetch('https://localhost:7067/Charger/getOutput', {
    method: 'GET',
    headers: {
      'accept': 'application/json'
    },
  })
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(err => console.log(err));
  
}

// // get the list of charging stations curl -X 'GET' \ 'https://localhost:7067/Charger/GetChargingStationList' \ -H 'accept: application/json'
// function getChargingStations() {
//     var chargingStations: ListType[];
//     var response = fetch('https://localhost:7067/Charger/GetChargingStationList', {
//         method: 'GET',
//         headers: {
//             'accept': 'application/json'
//         },
//     })
//         .then(response => response.json() as Promise<ListType[]>)
        
//         .then(data => console.log(data))
//         .catch(err => console.log(err));
//     chargingStations = response.then(data => data);
// }