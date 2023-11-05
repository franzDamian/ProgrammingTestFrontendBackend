import * as React from 'react';
import Container from '@mui/material/Container';
import Typography from '@mui/material/Typography';
import Box from '@mui/material/Box';
import Link from '@mui/material/Link';
import DiscreteSlider from './slider';
import { List } from './list';
import FormPropsTextFields from './inputBox';
import { ChargingEvents, ChargingValues, ExemplaryDay, TotalEnergyCharged } from './output';

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
  return (
    <Container maxWidth="sm">
      <Box sx={{ my: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          Material UI Vite.js example in TypeScript
        </Typography>
        <DiscreteSlider initial={8} max={30} min={1} title='Charging Stations' />
        <DiscreteSlider initial={100} max={200} min={20} steps={10} title='Multiplier for arival probalilty' unit='%'/>
        <FormPropsTextFields />
        <ChargingValues />
        <List data={testList} />
        <ExemplaryDay />
        <TotalEnergyCharged />
        <ChargingEvents />
        <Copyright />
      </Box>
    </Container>
  );
}