import { React, useEffect, useState, useReducer } from "react";
import axios from "axios";
import { Paper, TextField, Grid, Button } from "@mui/material";
import { HoursTable } from "../components/HoursTable";
var fs = require("browserify-fs");

export const Dash = (props) => {
  const [name, setName] = useReducer(
    (state, updates) => ({ ...state, ...updates }),
    { firstName: null, lastName: null }
  );
  const [data, setData] = useState();
  const [formattedData, setFormattedData] = useState();

  const request_headers = {
    "Access-Control-Allow-Origin": "*/*",
    "content-type": "application/json",
    Accept: "application/json",
  };

  useEffect(() => {
    console.log("formatting string:", data);
    if (data === null || data === undefined || typeof data == undefined) {
      setFormattedData({
        firstName: name.firstName,
        lastName: name.lastName,
        hours: [],
      });
      return;
    }

    var mapToList = [];
    Object.keys(data).forEach((key) => {
      mapToList.push({ weekEndingDate: key, hours: data[key].hours });
    });

    var formatted = {
      firstName: name.firstName,
      lastName: name.lastName,
      hours: mapToList,
    };
    console.log("Formatted string:", formatted);
    setFormattedData(formatted);
  }, [data, name, setData, setName]);

  useEffect(() => {
    // make api call
    if (name.firstName != null && name.lastName != null) {
      console.log("Making call");
      axios({
        method: "GET",
        url:
          "http://23.241.58.48:47275/api/Hours/name/" +
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
    var map = {};

    data.hours.forEach((element) => {
      map[element.weekEndingDate] = element;
    });

    console.log("result of reformatting:", map);

    return map;
  };

  const saveState = () => {
    console.log("Saving state:", data);

    var formattedPayload = {
      firstName: name.firstName,
      lastName: name.lastName,
      hours: Object.keys(data).map((key) => {
        return { weekEndingDate: key, hours: data[key].hours };
      }),
    };

    axios({
      method: "PUT",
      url:
        "http://23.241.58.48:47275/api/Hours/name/" +
        name.firstName +
        "/" +
        name.lastName,
      headers: request_headers,
      data: formattedPayload,
    })
      .then((response) => response.data)
      .then((data) => {
        setData(buildMapFromData(data));
      })
      .then(() => alert("Success!"))
      .catch((error) => console.log(error));
  };

  const updateFirstName = (e) => {
    console.log(e.target.value);
    setName({ firstName: e.target.value });
  };

  const updateLastName = (e) => {
    console.log(e.target.value);
    setName({ lastName: e.target.value });
  };

  const updateHoursData = (date, hours) => {
    setData((previous) => {
      var newHours = { ...previous };
      newHours[date] = { weekEndingDate: date, hours: hours };
      console.log("Updated:", newHours);
      return newHours;
    });
  };

  const getHoursForDate = (date) => {
    console.log("Getting hours for date:", date, data[date]);
    if (
      data[date] === undefined ||
      data[date] === null ||
      typeof data[date] === undefined
    ) {
      return "";
    }
    return data[date].hours;
  };

  const downloadReport = () => {
    axios({
      method: "GET",
      url: "http://23.241.58.48:47275/api/Excel",
      headers: request_headers,
    })
      .then(
        (response) =>
          new Blob([response.data], {
            type: "text/plain",
          })
      )
      .then((blob) => {
        const href = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = href;
        link.setAttribute("download", "report.csv"); //or any other extension
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      });
  };

  return (
    <div>
      <div id="braking div" style={{ height: "12vh" }} />
      <Paper
        style={{ width: "75vw", margin: "auto", width: "50%", padding: "10px" }}
      >
        <Grid
          container
          direction="row"
          justifyContent="center"
          alignItems="center"
          spacing={2}
        >
          <Grid item lg={2}>
            <TextField
              id="outlined-basic"
              label="First Name"
              variant="outlined"
              style={{ width: "100%" }}
              onChange={updateFirstName}
            />
          </Grid>
          <Grid item lg={2}>
            <TextField
              id="outlined-basic"
              label="Last Name"
              variant="outlined"
              style={{ width: "100%" }}
              onChange={updateLastName}
            />
          </Grid>
          {data === null || data === undefined ? (
            <div>
              {console.log(
                "Not rendering table because data is not available",
                data
              )}
            </div>
          ) : (
            <Grid item lg={12}>
              <center>
                <HoursTable
                  updateHours={updateHoursData}
                  saveChanges={saveState}
                  getHoursForDate={getHoursForDate}
                />
              </center>
            </Grid>
          )}
          <Grid item lg={12} style={{ textAlign: "center" }}>
            {JSON.stringify(formattedData)}
          </Grid>
        </Grid>
        <Button onClick={downloadReport}>Download Report</Button>
      </Paper>
    </div>
  );
};
