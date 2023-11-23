import { styled } from "@mui/material/styles";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell, { tableCellClasses } from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import { NumberOfCharginStationWithPowerWithId } from "./charging-stations-add";
import { Box, Button, IconButton } from "@mui/material";
import { Delete as DeleteIcon } from "@mui/icons-material";
import { ChargerClient } from "../../infrastructure/api";
import { ChargingStationBackendClient } from "../../infrastructure/generated/client.g";

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
	"&:nth-of-type(odd)": {
		backgroundColor: theme.palette.action.hover,
	},
	// hide last border
	"&:last-child td, &:last-child th": {
		border: 0,
	},
}));

type ChargingStationProps = {
	readonly rows: NumberOfCharginStationWithPowerWithId[];
	readonly handleDelete: (_: NumberOfCharginStationWithPowerWithId) => void;
};

export default function ChargingStationTable(props: ChargingStationProps) {
	const handleSubmit = () => {
		ChargerClient.postSimulationInput({
			arrivalProbabilityMultiplier: 1,
			averageConsumptionOfCars: 18,
			chargingStations: props.rows.flatMap((el) =>
				[...Array(el.count).keys()].map(
					(_) =>
						({
							chargingPower: el.power,
						} as ChargingStationBackendClient.ChargingStationDto)
				)
			),
		} as ChargingStationBackendClient.SimulationInputDto)
			.then(() => console.log("done"))
			.catch(() => console.log("error"));
	};
	return (
		<Box
			sx={{ padding: 2 }}
			display="flex"
			justifyContent="space-between"
			flexDirection="column"
		>
			<TableContainer component={Paper}>
				<Table sx={{}} aria-label="customized table">
					<TableHead>
						<TableRow>
							<StyledTableCell>Charging Station</StyledTableCell>
							<StyledTableCell align="right">Power</StyledTableCell>
							<StyledTableCell></StyledTableCell>
						</TableRow>
					</TableHead>
					<TableBody>
						{props.rows.map((row, index) => (
							<StyledTableRow key={index}>
								<StyledTableCell component="th" scope="row">
									{row.count}
								</StyledTableCell>
								<StyledTableCell align="right">{row.power}</StyledTableCell>
								<StyledTableCell>
									<IconButton
										aria-label="delete"
										size="small"
										onClick={() => props.handleDelete(row)}
									>
										<DeleteIcon color="error" fontSize="inherit" />
									</IconButton>
								</StyledTableCell>
							</StyledTableRow>
						))}
					</TableBody>
				</Table>
			</TableContainer>
			<></>
			<Button variant="contained" onClick={() => handleSubmit()}>
				Submit
			</Button>
		</Box>
	);
}
