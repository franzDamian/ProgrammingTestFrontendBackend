import * as React from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import { InputAdornment } from '@mui/material';

export default function FormPropsTextFields() {
  return (
    <Box
      component="form"
      sx={{
        '& .MuiTextField-root': { m: 1, width: '25ch' },
      }}
      noValidate
      autoComplete="off"
    >
      <div>
        
        <TextField
                  id="outlined-number"
                  label="consumption of the cars"
                  type="number"
                  defaultValue={18}
                  InputLabelProps={{
                      shrink: true,
                  }}
                  InputProps={{
                      endAdornment: <InputAdornment position="start">kW</InputAdornment>
                  }}
        />
      </div>
    </Box>
  );
}