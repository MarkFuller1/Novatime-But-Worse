import { TextField } from "@mui/material";
import React from "react";

export const DateDetails = (props) => {
  return (
    <div>
      For {props.date} you have recorded{" "}
      <TextField
        id="outlined-basic"
        label="First Name"
        variant="outlined"
        style={{ width: "100%" }}
        onChange={handleHoursEntry}
        defaultValue={
          props.hours === undefined || props.hours === null
            ? "none"
            : props.hours
        }
      />
    </div>
  );
};
