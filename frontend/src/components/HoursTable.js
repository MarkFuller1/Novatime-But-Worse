import { React, useEffect } from "react";
import { DataGrid, GridColumns, GridRowsProp } from "@mui/x-data-grid";
import Grid from "@mui/material/Grid";
import Paper from "@mui/material/Paper";
import Button from "@mui/material/Button";

export const HoursTable = (props) => {
  console.log(props);

  const columns = [
    {
      field: "hours",
      headerName: "Hours",
      type: "number",
      width: "500",
      alignItems: "left",
      editable: true,
    },
    {
      field: "weekEndingDate",
      headerName: "Date",
      width: "100",
      editable: true,
    },
  ];

  const alterDates = (hours) => {
    var altered = hours.map((hour) => ({
      ...hour,
      hours: new Date(hour.hours),
    }));
    console.log(altered);
    return altered;
  };

  return (
    <Paper style={{ width: "50%" }}>
      <center>
        <Grid
          item
          container
          direction="row"
          justifyContent="center"
          alignItems="center"
          spacing={2}
          lg={12}
          style={{ width: "100%", height: "30%" }}
        >
          <Grid item lg={12}>
            <div style={{ height: 300, width: "100%" }}>
              <DataGrid
                style={{ width: "80%" }}
                getRowId={(row) => row.weekEndingDate}
                rows={alterDates(props.rows.hours)}
                columns={columns}
                experimentalFeatures={{ newEditingApi: true }}
              />
            </div>
          </Grid>
        </Grid>
      </center>
      {/* <Button onClick={props.addRow}>Add Row</Button> */}
    </Paper>
  );
};
