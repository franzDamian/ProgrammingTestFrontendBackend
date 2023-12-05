import Box from "@mui/material/Box";
import TextField from "@mui/material/TextField";
import { InputAdornment } from "@mui/material";

type FormPropsTextFieldsProps = {
	readonly label: string;
	readonly type: string;
	readonly defaultValue: number;
	readonly min: number;
	readonly max: number;
	readonly step: number;
	readonly endAdornment: string;
	readonly shrink: boolean;
};

export const FormPropsTextFields = (props: FormPropsTextFieldsProps) => {
	return (
		<Box component="form" noValidate autoComplete="off">
			<div>
				<TextField
					id="outlined-number"
					label={props.label}
					type={props.type}
					defaultValue={props.defaultValue}
					InputLabelProps={{
						shrink: props.shrink,
					}}
					InputProps={{
						endAdornment: (
							<InputAdornment position="start">
								{props.endAdornment}
							</InputAdornment>
						),
					}}
				/>
			</div>
		</Box>
	);
};
