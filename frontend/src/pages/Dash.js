import { React, useEffect, useState, useReducer } from "react";
import axios from "axios";
import { TextField, Grid } from "@mui/material";
import { HoursTable } from "../components/HoursTable";

export const Dash = (props) => {
  const [name, setName] = useReducer(
    (state, updates) => ({ ...state, ...updates }),
    { firstName: null, lastName: null }
  );
  const [user, setUser] = useState({ firstName: null, lastName: null });
  const [data, setData] = useState();

  const request_headers = {
    "Access-Control-Allow-Origin": "*/*",
    "content-type": "application/json",
    Accept: "application/json",
  };

  useEffect(() => {
    // make api call
    if (name.firstName != null && name.lastName != null) {
      console.log("Making call");
      axios({
        method: "GET",
        url:
          "http://localhost:47275/api/Hours/name/" +
          name.firstName +
          "/" +
          name.lastName,
        headers: request_headers,
      })
        .then((result) => {
          var resultData = result.data;
          console.log("Received Data:", resultData);
          return resultData;
        })
        .then((data) => setData(data))
        .catch((error) => console.log(error));
    }
  }, [name, setName]);

  const updateFirstName = (e) => {
    console.log(e.target.value);
    setName({ firstName: e.target.value });
  };

  const updateLastName = (e) => {
    console.log(e.target.value);
    setName({ lastName: e.target.value });
  };

  return data == null || typeof data == undefined ? (
    <div />
  ) : (
    <Grid
      container
      direction="row"
      justifyContent="center"
      alignItems="center"
      spacing={2}
    >
      <Grid item lg={2} />
      <Grid item lg={4}>
        <TextField
          id="outlined-basic"
          label="First Name"
          variant="outlined"
          style={{ width: "100%" }}
          onChange={updateFirstName}
        />
      </Grid>
      <Grid item lg={4}>
        <TextField
          id="outlined-basic"
          label="Last Name"
          variant="outlined"
          style={{ width: "100%" }}
          onChange={updateLastName}
        />
      </Grid>
      <Grid item lg={2} />
      <Grid item lg={12} style={{ textAlign: "center" }}>
        {name.firstName + " " + name.lastName}
      </Grid>
      <Grid item lg={12} style={{ textAlign: "center" }}>
        {data.firstName + " " + data.lastName + " " + data.hours}
      </Grid>
      {data.hours == undefined || data.hours == null ? (
        <div />
      ) : (
        <HoursTable
          rows={data}
          // addRow={() =>
          //   setData((prev) => [
          //     ...prev.hours,
          //     { date: undefined, weekEndingDate: undefined },
          //   ])
          // }
        />
      )}
    </Grid>
  );
};
