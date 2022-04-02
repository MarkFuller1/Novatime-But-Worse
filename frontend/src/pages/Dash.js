import { React, useEffect, useState, useReducer } from "react";
import axios from "axios";
import { TextField, Grid } from "@mui/material";
import { HoursTable } from "../components/HoursTable";

export const Dash = (props) => {
  const [name, setName] = useReducer(
    (state, updates) => ({ ...state, ...updates }),
    { firstName: null, lastName: null }
  );
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
        .then((response) => {
          if (response.status !== 200) {
            // dont do anything
            throw response;
          } else {
            var resultData = response.data;
            console.log("Received Data:", resultData);
            return resultData;
          }
        })
        .then((data) => setData(buildMapFromData(data)))
        .catch((error) => console.log("Request issue:", error));
    }
  }, [name, setName]);

  const buildMapFromData = (data) => {
    console.log("Reformatting:", data);
    if (
      data === undefined ||
      data === null ||
      typeof data === undefined ||
      data === ""
    ) {
      console.log("setting data to undefined");
      // setData(undefined);
      return;
    }
    var map = new Map();

    var result = { ...data };
    delete result.hours;

    data.hours.forEach((element) => {
      map.set(element.weekEndingDate, element);
    });

    result.hours = map;

    console.log("result of reformatting:", result);

    return result;
  };

  const updateFirstName = (e) => {
    console.log(e.target.value);
    setName({ firstName: e.target.value });
  };

  const updateLastName = (e) => {
    console.log(e.target.value);
    setName({ lastName: e.target.value });
  };

  const addNewHours = (hours, date) => {
    axios({
      method: "PUT",
      url:
        "http://localhost:47275/api/Hours/name/" +
        name.firstName +
        "/" +
        name.lastName +
        "/" +
        hours +
        "/" +
        date,
      headers: request_headers,
    })
      .then((response) => response.data)
      .then((data) => {
        console.log("updated entry: " + data);
        setData(buildMapFromData(data));
      })
      .catch((error) => console.log(error));
  };

  return (
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
      {data == null || data == undefined ? (
        <div>
          {console.log(
            "Not rendering table because data is not available",
            data
          )}
        </div>
      ) : (
        <HoursTable handleCalendar={addNewHours} />
      )}
    </Grid>
  );
};
