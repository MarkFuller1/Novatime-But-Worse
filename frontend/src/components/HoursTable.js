import { React, useEffect } from "react";
import { DataGrid, GridColumns, GridRowsProp } from "@mui/x-data-grid";
import Grid from "@mui/material/Grid";
import Paper from "@mui/material/Paper";
import Button from "@mui/material/Button";
import { DateTime } from "luxon";
import Calendar from "react-calendar";

export const HoursTable = (props) => {
  console.log("Props in hours table", props);

  const columns = [
    {
      field: "weekEndingDate",
      headerName: "Week of",
      width: "500",
      editable: true,
    },
    {
      field: "hours",
      headerName: "Hours",
      type: "number",
      alignItems: "left",
      editable: true,
    },
  ];

  const formatDate = (date) => {
    console.log("Formatting date:", date);
    var day = date.getDay();
    var month = date.getMonth();
    var year = date.getFullYear();

    return month + " " + day + " " + year;
  };

  const alterDates = (hours) => {
    console.log("Ready to format dates:", hours);
    var altered = hours.map((hour) => ({
      ...hour,
      weekEndingDate: DateTime.fromMillis(hour.weekEndingDate).toFormat(
        "EEEE MMMM d, yyyy"
      ),
    }));
    console.log("Altered Dates:", altered);
    return altered;
  };

  const handleCalendar = (event) => {
    console.log("Calendar:", event);
    // props.data.hours[]
  };

  return (
    <Paper style={{ width: "50%" }}>
      <Calendar onChange={handleCalendar} />
    </Paper>
  );
};
