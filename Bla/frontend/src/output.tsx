import React from 'react';
import { Card, Typography } from '@mui/material';
import { ListType } from './App';
import { Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material';

// Component to display number of charging events
export const ChargingEvents = () => {
  // Fetch and process data here
  const data = [
    { period: 'Year', events: 1200 },
    { period: 'Month', events: 100 },
    { period: 'Week', events: 25 },
    { period: 'Day', events: 4 },
  ];

  return (
    <Card>
      <TableContainer component={Paper}>
        <Table sx={{ minWidth: 200 }} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell>Period</TableCell>
              <TableCell align="right">Number of Charging Events</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {data.map((row) => (
              <TableRow key={row.period}>
                <TableCell component="th" scope="row">
                  {row.period}
                </TableCell>
                <TableCell align="right">{row.events}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Card>
  );
};

type ListProps = {
  data: ListType[];
};

// Component to display charging values per chargepoint
export const ChargingValues = (props: ListProps) => {
  // Fetch and process data here

  return (
    <Card sx={{ paddingY: 1 , paddingX: 2 }}>
      <Typography  variant="h6">Charging Values per Chargepoint</Typography>
      {
        props.data.map((item, index) => <Typography key={index}>{item.key}: {item.value}</Typography>)
      }
    </Card>
  );
};

// Component to display an exemplary day
export const ExemplaryDay = () => {
  // Fetch and process data here

  return (
    <Card>
      <Typography variant="h6">Exemplary Day</Typography>
      {/* Display data here 
        Data should be displayed in a table with the following columns:
        - Time
        - Number of cars charging
        - Total energy charged
      */
      
      }
    </Card>
  );
};

// Component to display total energy charged
export const TotalEnergyCharged = () => {
  // Fetch and process data here

  return (
    <Card>
      <Typography variant="h6">Total Energy Charged</Typography>
      {/* Display data here */
        // Generate a table with the following columns: total energy charged per year/month/week/day
        
      }
    </Card>
  );
};

//// Component to display number of charging events
//export const ChargingEvents = () => {
//  // Fetch and process data here
//
//  return (
//    <Card>
//      <Typography variant="h6">Number of Charging Events</Typography>
//      {/* Display data here 
//        Data should be displayed in a table with the following columns:
//        - Number of charging events per year/month/week/day
//      */
//        // Generate a table with the following columns: number of charging events per year/month/week/day
//
//
//      }
//    </Card>
//  

