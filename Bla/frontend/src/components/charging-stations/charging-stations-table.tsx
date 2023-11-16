import { styled } from '@mui/material/styles';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell, { tableCellClasses } from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import { NumberOfCharginStationWithPower } from './charging-stations-add';

const StyledTableCell = styled(TableCell)(({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
        backgroundColor: theme.palette.common.black,
        color: theme.palette.common.white,
    },
    [`&.${tableCellClasses.body}`]: {
        fontSize: 14,
    },
}));

const StyledTableRow = styled(TableRow)(({ theme }) => ({
    '&:nth-of-type(odd)': {
        backgroundColor: theme.palette.action.hover,
    },
    // hide last border
    '&:last-child td, &:last-child th': {
        border: 0,
    },
}));


type ChargingStationProps = {
    readonly rows: NumberOfCharginStationWithPower[];
}

export default function ChargingStationTable(props: ChargingStationProps) {
    return (

        <TableContainer component={Paper}>
            <Table sx={{ }} aria-label="customized table">
                <TableHead>
                    <TableRow>
                        <StyledTableCell>Charging Station</StyledTableCell>
                        <StyledTableCell align="right">Power</StyledTableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {props.rows.map((row, index) => (
                        <StyledTableRow key={index}>
                            <StyledTableCell component="th" scope="row">
                                {row.count}
                            </StyledTableCell>
                            <StyledTableCell align="right">{row.power}</StyledTableCell>
                        </StyledTableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
}