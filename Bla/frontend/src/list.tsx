import { Typography } from "@mui/material";
import { ListType } from "./App";


type ListProps = {
    data: ListType[];
}

export const List = (props: ListProps) => {
    return (
        <>
            {props.data.map((item, index) => <Typography key={index}>{item.key}: {item.value}</Typography>)}
        </>
    )
}